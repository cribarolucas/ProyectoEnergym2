// General Parameters for this app, used during initialization
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
            // support grid snapping when dragging and when resizing
            "draggingTool.isGridSnapEnabled": true,
            "draggingTool.gridSnapCellSpot": go.Spot.Center,
            "resizingTool.isGridSnapEnabled": true,
            allowDrop: true,  // handle drag-and-drop from the Palette
            //// For this sample, automatically show the state of the diagram's model on the page
            //"ModelChanged": function (e) {
            //    if (e.isTransactionFinished) {
            //        document.getElementById("savedModel").textContent = myDiagram.model.toJson();
            //    }
            //},
            "animationManager.isEnabled": false,
            "undoManager.isEnabled": true
        });
    // Regular Nodes represent items to be put onto racks.
    // Nodes are currently resizable, but if that is not desired, just set resizable to false.
    myDiagram.nodeTemplate =
      $(go.Node, "Auto",
        {
            resizable: false, resizeObjectName: "SHAPE",
            // because the gridSnapCellSpot is Center, offset the Node's location
            locationSpot: new go.Spot(0, 0, CellSize.width / 2, CellSize.height / 2),
            // provide a visual warning about dropping anything onto an "item"
            mouseDragEnter: function (e, node) {
                e.handled = true;
                node.findObject("SHAPE").fill = "red";
                highlightGroup(node.containingGroup, false);
            },
            mouseDragLeave: function (e, node) {
                node.updateTargetBindings();
            },
            mouseDrop: function (e, node) {  // disallow dropping anything onto an "item"
                node.diagram.currentTool.doCancel();
            }
        },
        // always save/load the point that is the top-left corner of the node, not the location
        new go.Binding("position", "pos", go.Point.parse).makeTwoWay(go.Point.stringify),
        // this is the primary thing people see
        $(go.Shape, "Rectangle",
          {
              name: "SHAPE",
              fill: "white",
              minSize: CellSize,
              desiredSize: CellSize  // initially 1x1 cell
          },
          new go.Binding("fill", "color"),
          new go.Binding("desiredSize", "size", go.Size.parse).makeTwoWay(go.Size.stringify)),
        // with the textual key in the middle
        $(go.TextBlock,
          { alignment: go.Spot.Center, font: 'bold 16px sans-serif' },
          new go.Binding("text", "key"))
      );  // end Node
    // Groups represent racks where items (Nodes) can be placed.
    // Currently they are movable and resizable, but you can change that
    // if you want the racks to remain "fixed".
    // Groups provide feedback when the user drags nodes onto them.
    function highlightGroup(grp, show) {
        if (!grp) return;
        if (show) {  // check that the drop may really happen into the Group
            var tool = grp.diagram.toolManager.draggingTool;
            var map = tool.draggedParts || tool.copiedParts;  // this is a Map
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
        $(go.Shape, "Rectangle",  // the rectangular shape around the members
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
    // decide what kinds of Parts can be added to a Group
    myDiagram.commandHandler.memberValidation = function (grp, node) {
        if (grp instanceof go.Group && node instanceof go.Group) return false;  // cannot add Groups to Groups
        // but dropping a Group onto the background is always OK
        return true;
    };
    // what to do when a drag-drop occurs in the Diagram's background
    myDiagram.mouseDragOver = function (e) {
        if (!AllowTopLevel) {
            // but OK to drop a group anywhere
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
    // start off with four "racks" that are positioned next to each other
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
    // initialize the products Palette
    myPaletteProducts =
      $(go.Palette, "myPaletteProducts",
        { // share the templates with the main Diagram
            nodeTemplate: myDiagram.nodeTemplate,
            groupTemplate: myDiagram.groupTemplate,
            layout: $(go.GridLayout)
        });
    var blue = '#81D4FA';
    // specify the contents of the Palette
    myPaletteProducts.model = new go.GraphLinksModel([
        { key: "Hom", color: blue, size: "30 30" },
        { key: "Dor", color: blue, size: "30 30" },
        { key: "Bice", color: blue, size: "60 30" },
        { key: "Pec", color: blue, size: "60 30" },
        { key: "Bici", color: blue, size: "30 60" },
        { key: "Cin", color: blue, size: "30 90" },        
        { key: "Pie", color: blue, size: "30 60" }
    ]);

    // initialize the occupied space Palette
    myPaletteOS =
      $(go.Palette, "myPaletteOS",
        { // share the templates with the main Diagram
            nodeTemplate: myDiagram.nodeTemplate,
            groupTemplate: myDiagram.groupTemplate,
            layout: $(go.GridLayout)
        });
    var red = '#F21010';
    // specify the contents of the Palette
    myPaletteOS.model = new go.GraphLinksModel([
      { key: "EO1", color: red },
      { key: "EO2", color: red, size: "30 60" },
      { key: "EO3", color: red, size: "60 30" },
      { key: "EO4", color: red, size: "60 60" }
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