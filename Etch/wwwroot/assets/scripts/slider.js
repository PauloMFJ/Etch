/*! file: slider.js | author: Etch | website: ? */
var slider = document.getElementById("slider");
var output = document.getElementById("progress");
output.innerHTML = slider.value;

slider.oninput = function() {
    output.innerHTML = this.value;
}