
function submitDeleteForm(imgElement) {
    var form = imgElement.closest('form');
    form.submit();
}

var isAllCheck = false;
function togglecheckboxes(cn) {

    var cbarray = document.getElementsByName(cn);
    for (var i = 0; i < cbarray.length; i++) {

        cbarray[i].checked = !isAllCheck
    }
    isAllCheck = !isAllCheck;
}