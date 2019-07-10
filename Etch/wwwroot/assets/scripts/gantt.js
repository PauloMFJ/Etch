/*! file: gantt.js | author: Etch | website: ? */
window.addEventListener("load", function () {
    var picker = new Piklor(".colour-picker", [
        "#FFA3A3"
        , "#FF0000"
        , "#FF9AF4"
        , "#FF00E4"
        , "#CD8CFF"
        , "#9000FF"
        , "#9893FF"
        , "#0C00FF"
        , "#97F4FF"
        , "#00E4FF"
        , "#8EFF91"
        , "#00FF06"
        , "#FEFF96"
        , "#FCFF00"
        , "#FFD297"
        , "#FF9000"
        , "#FFFFFF"
        , "#B7B7B7"
        , "#555555"
        , "#252525"], ),
        wrapper = picker.getElm(".colour-picker-wrapper");
    input = picker.getElm(".colour-input");
    wrapper.style.backgroundColor = input.value;

    picker.colorChosen(function (colour) {
        input.value = colour;
        wrapper.style.backgroundColor = input.value;
    });
});

$("#close").click(function () {
    $('#popup').removeClass('show');
});