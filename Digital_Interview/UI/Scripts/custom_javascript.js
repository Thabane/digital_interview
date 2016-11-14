$(document).ready(function () {

    $('a[data-toggle=modal], button[data-toggle=modal]').click(function () {

        var data_resId = '';
        var data_subId= '';

        if (typeof $(this).data('resid') !== 'undefined') {

            data_resId = $(this).data('resid');
        }
        if (typeof $(this).data('subid') !== 'undefined') {

            data_subId = $(this).data('subid');
        }

        $('#resourceid').val(data_resId);
        $('#subscriptionid').val(data_subId);
    })
});

function RedirectFunc() {
    var resID = document.getElementById("resourceid").value;
    var subID = document.getElementById("subscriptionid").value;
    window.location ="/Resources/UseResource?resId=" + resID + "&subcriptionID=" + subID;
}