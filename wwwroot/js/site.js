// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function preview() {

    frame.src = URL.createObjectURL(event.target.files[0]);
    var trash = document.getElementById("trash-btn");
    if (trash.style.display === "none") {
        trash.style.display = "block";
    }
} export {preview}

function clearImage() {
    document.getElementById('formFile').value = null;
    frame.src = "";
    var trash = document.getElementById("trash-btn");
    if (trash.style.display === "block") {
        trash.style.display = "none";
    }
} export { clearImage }

