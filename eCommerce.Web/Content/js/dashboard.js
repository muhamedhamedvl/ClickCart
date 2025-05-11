//$("#menu-toggle").click(function (e) {
//    e.preventDefault();
//    $("#wrapper").toggleClass("toggled");
//});



//function activatePageLink() {
//    $(".list-group-item", "#sidebar-wrapper").removeClass("active");

//    var activePage = $("#activePage").val();

//    $("a.list-group-item[data-page="+activePage+"]").addClass("active");
//}

function addLoader(containerID) {
    $("<div class='loading-overlay'><div class='spinner-div d-flex justify-content-center'><div class='align-self-center spinner-border' role='status'><span class='sr-only'>Loading...</span></div></div></div>").appendTo($("#" + containerID).css("position", "relative"));
}

function removeLoader(containerID) {
    $(".loading-overlay", "#" + containerID).remove();
}

function updateEditFields() {
    for (instance in CKEDITOR.instances) {
        CKEDITOR.instances[instance].updateElement();
    }
}
function removeThis(elem) {
    $(elem).remove();
}

function removeImage(elem) {
    $(elem).parents(".thisImage").remove();
}

$(document).ready(function () {
    $("#btnLogOff").click(function () {
        $("#logOffForm").submit();
    });

    $("#sidebarToggle, #sidebarToggleTop").on('click', function (e) {
        $("body").toggleClass("sidebar-toggled");
        $(".sidebar").toggleClass("toggled");

        if ($(".sidebar").hasClass("toggled")) {
            $('.sidebar .collapse').collapse('hide');
        }
    });

    $(".ddlanguages").msDropdown({ roundedBorder: false });
    $(".ddlanguages").change(function () {
        var redirectURL = $(this).val();

        if (redirectURL) {
            window.location.href = redirectURL;
        }
    });
});