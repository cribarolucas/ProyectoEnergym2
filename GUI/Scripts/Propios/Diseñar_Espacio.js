﻿// Parámetros generales para esta aplicación, utilizados durante la inicialización
var AllowTopLevel = false;
var CellSize = new go.Size(30, 30);

//window.onload = init();

function init() {
    var $ = go.GraphObject.make;
    myDiagram =
      $(go.Diagram, "myDiagramDiv",
        {
            grid: $(go.Panel, "Grid",
                    { gridCellSize: CellSize },
                    $(go.Shape, "LineH", { stroke: "lightgray" }),
                    $(go.Shape, "LineV", { stroke: "lightgray" })
                  ),
            // admite el ajuste de cuadrícula al arrastrar y al cambiar el tamaño
            "draggingTool.isGridSnapEnabled": true,
            "draggingTool.gridSnapCellSpot": go.Spot.Center,
            "resizingTool.isGridSnapEnabled": true,
            allowDrop: true,  // manejar arrastrar y soltar desde la paleta
            //// For this sample, automatically show the state of the diagram's model on the page
            //"ModelChanged": function (e) {
            //    if (e.isTransactionFinished) {
            //        document.getElementById("savedModel").textContent = myDiagram.model.toJson();
            //    }
            //},
            "animationManager.isEnabled": false,
            "undoManager.isEnabled": true
        });
    // Los nodos regulares representan elementos para colocar en bastidores
    // Los nodos son actualmente redimensionables, pero si eso no se desea, simplemente configure redimensionable en falso.
    myDiagram.nodeTemplate =
      $(go.Node, "Auto",
        {
            resizable: false, resizeObjectName: "SHAPE",
            // porque gridSnapCellSpot es Center, compensa la ubicación del nodo
            locationSpot: new go.Spot(0, 0, CellSize.width / 2, CellSize.height / 2),
            // proporcionar una advertencia visual sobre dejar caer cualquier cosa en un "elemento"
            mouseDragEnter: function (e, node) {
                e.handled = true;
                node.findObject("SHAPE").fill = "red";
                highlightGroup(node.containingGroup, false);
            },
            mouseDragLeave: function (e, node) {
                node.updateTargetBindings();
            },
            mouseDrop: function (e, node) {  // no permitir dejar caer nada sobre un "elemento"
                node.diagram.currentTool.doCancel();
            }
        },
        // siempre guarde / cargue el punto que es la esquina superior izquierda del nodo, no la ubicación
        new go.Binding("position", "pos", go.Point.parse).makeTwoWay(go.Point.stringify),
        // esto es lo principal que la gente ve
        $(go.Shape, "Rectangle",
          {
              name: "SHAPE",
              fill: "white",
              minSize: CellSize,
              desiredSize: CellSize  // initially 1x1 cell
          },
          new go.Binding("fill", "color"),
          new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify)),
        // con el texto en el medio
        $(go.TextBlock,
          { alignment: go.Spot.Center, font: 'bold 16px sans-serif' },
          new go.Binding("text", "key"))
      );  // fin del nodo
    // Los grupos representan bastidores donde se pueden colocar elementos (nodos).
    // Actualmente son móviles y redimensionables, pero puedes cambiar eso
    // si desea que los bastidores permanezcan "fixed".
    // Los grupos proporcionan comentarios cuando el usuario arrastra nodos hacia ellos.
    function highlightGroup(grp, show) {
        if (!grp) return;
        if (show) {  // compruebe que el "drop" realmente pueda ocurrir en el Grupo
            var tool = grp.diagram.toolManager.draggingTool;
            var map = tool.draggedParts || tool.copiedParts;  // Esto es un mapa
            if (grp.canAddMembers(map.toKeySet())) {
                grp.isHighlighted = true;
                return;
            }
        }
        grp.isHighlighted = false;
    }
    var groupFill = "rgba(128,128,128,0.2)";
    var groupStroke = "gray";
    var dropFill = "rgba(128,255,255,0.2)";
    var dropStroke = "red";
    myDiagram.groupTemplate =
      $(go.Group,
        {
            layerName: "Background",
            resizable: true, resizeObjectName: "SHAPE",
            // because the gridSnapCellSpot is Center, offset the Group's location
            locationSpot: new go.Spot(0, 0, CellSize.width / 2, CellSize.height / 2)
        },
        // always save/load the point that is the top-left corner of the node, not the location
        new go.Binding("position", "pos", go.Point.parse).makeTwoWay(go.Point.stringify),
        { // what to do when a drag-over or a drag-drop occurs on a Group
            mouseDragEnter: function (e, grp, prev) { highlightGroup(grp, true); },
            mouseDragLeave: function (e, grp, next) { highlightGroup(grp, false); },
            mouseDrop: function (e, grp) {
                var ok = grp.addMembers(grp.diagram.selection, true);
                if (!ok) grp.diagram.currentTool.doCancel();
            }
        },
        $(go.Shape, "Rectangle",  // la forma rectangular alrededor de los miembros
          {
              name: "SHAPE",
              fill: groupFill,
              stroke: groupStroke,
              minSize: new go.Size(CellSize.width * 2, CellSize.height * 2)
          },
          new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify),
          new go.Binding("fill", "isHighlighted", function (h) { return h ? dropFill : groupFill; }).ofObject(),
          new go.Binding("stroke", "isHighlighted", function (h) { return h ? dropStroke : groupStroke; }).ofObject())
      );
    // decidir qué tipo de piezas se pueden agregar a un grupo
    myDiagram.commandHandler.memberValidation = function (grp, node) {
        if (grp instanceof go.Group && node instanceof go.Group) return false;  // no puede agregar grupos a grupos
        // pero dejar un grupo de fondo siempre está bien
        return true;
    };
    // qué hacer cuando se produce un arrastrar y soltar en el fondo del diagrama
    myDiagram.mouseDragOver = function (e) {
        if (!AllowTopLevel) {
            // pero está bien dejar un grupo en cualquier lugar
            if (!e.diagram.selection.all(function (p) { return p instanceof go.Group; })) {
                e.diagram.currentCursor = "not-allowed";
            }
        }
    };
    myDiagram.mouseDrop = function (e) {
        if (AllowTopLevel) {
            // when the selection is dropped in the diagram's background,
            // make sure the selected Parts no longer belong to any Group
            if (!e.diagram.commandHandler.addTopLevelParts(e.diagram.selection, true)) {
                e.diagram.currentTool.doCancel();
            }
        } else {
            // disallow dropping any regular nodes onto the background, but allow dropping "racks"
            if (!e.diagram.selection.all(function (p) { return p instanceof go.Group; })) {
                e.diagram.currentTool.doCancel();
            }
        }
    };
    // comenzar con cuatro "bastidores" que se colocan uno al lado del otro
    myDiagram.model = new go.GraphLinksModel([
    { key: "G1", isGroup: true, pos: "0 0", size: "330 270" },
    { key: "G2", isGroup: true, pos: "330 0", size: "330 270" },
    { key: "G3", isGroup: true, pos: "0 270", size: "330 270" },
    { key: "G4", isGroup: true, pos: "330 270", size: "330 270" }
    ]);
    // this sample does not make use of any links
    jQuery("#accordion").accordion({
        activate: function (event, ui) {
            myPaletteProducts.requestUpdate();
            myPaletteOS.requestUpdate();
        }
    });
    // inicializar la paleta de productos
    myPaletteProducts =
      $(go.Palette, "myPaletteProducts",
        { // compartir las plantillas con el diagrama principal
            nodeTemplate: myDiagram.nodeTemplate,
            groupTemplate: myDiagram.groupTemplate,
            layout: $(go.GridLayout)
        });
    var orange = '#FFBF00';
    // especificar el contenido de la paleta
    myPaletteProducts.model = new go.GraphLinksModel([
        { key: "Hom", color: orange, size: "30 30" },
        { key: "Dor", color: orange, size: "30 30" },
        { key: "Bice", color: orange, size: "60 30" },
        { key: "Pec", color: orange, size: "60 30" },
        { key: "Bici", color: orange, size: "30 60" },
        { key: "Cin", color: orange, size: "30 90" },
        { key: "Pie", color: orange, size: "30 60" }
    ]);

    // inicializar la paleta de productos
    myPaletteOS =
      $(go.Palette, "myPaletteOS",
        { // compartir las plantillas con el diagrama principal
            nodeTemplate: myDiagram.nodeTemplate,
            groupTemplate: myDiagram.groupTemplate,
            layout: $(go.GridLayout)
        });
    var white = '#FFFFFF';
    // especificar los contenidos de las paletas
    myPaletteOS.model = new go.GraphLinksModel([
      { key: "EO1", color: white },
      { key: "EO2", color: white, size: "30 60" },
      { key: "EO3", color: white, size: "60 30" },
      { key: "EO4", color: white, size: "60 60" }
    ]);

    //// initialize the tall items Palette
    //myPaletteTall =
    //  $(go.Palette, "myPaletteTall",
    //    { // share the templates with the main Diagram
    //        nodeTemplate: myDiagram.nodeTemplate,
    //        groupTemplate: myDiagram.groupTemplate,
    //        layout: $(go.GridLayout)
    //    });
    //// specify the contents of the Palette
    //myPaletteTall.model = new go.GraphLinksModel([
    //  { key: "EO", color: red, size: "30 60" }
    //]);

    //// initialize the wide items Palette
    //myPaletteWide =
    //  $(go.Palette, "myPaletteWide",
    //    { // share the templates with the main Diagram
    //        nodeTemplate: myDiagram.nodeTemplate,
    //        groupTemplate: myDiagram.groupTemplate,
    //        layout: $(go.GridLayout)
    //    });
    //// specify the contents of the Palette
    //myPaletteWide.model = new go.GraphLinksModel([
    //  { key: "EO", color: red, size: "60 30" }
    //]);

    //// initialize the big items Palette
    //myPaletteBig =
    //  $(go.Palette, "myPaletteBig",
    //    { // share the templates with the main Diagram
    //        nodeTemplate: myDiagram.nodeTemplate,
    //        groupTemplate: myDiagram.groupTemplate,
    //        layout: $(go.GridLayout)
    //    });
    //// specify the contents of the Palette
    //myPaletteBig.model = new go.GraphLinksModel([
    //  { key: "EO", color: red, size: "60 60" }
    //]);
}