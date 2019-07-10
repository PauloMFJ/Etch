/*! file: file.js | author: Etch | website: ? */
var inputs = document.querySelectorAll('#fileUpload');
Array.prototype.forEach.call(inputs, function (input) {
    // Get label information
    var label = input.nextElementSibling, labelVal = label.innerHTML;

    // On input change event listener
    input.addEventListener('change', function (e) {
        var fileName = e.target.value.split('\\').pop();

        // If file name exists change message to file name, else restore default message
        if (fileName) {
            label.querySelector('span').innerHTML = fileName + " uploaded!";
            label.classList.add('uploaded');
        }
        else {
            label.innerHTML = labelVal;
            label.classList.remove('uploaded');
        }
    });
});