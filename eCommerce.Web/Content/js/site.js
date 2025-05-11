/*  ---------------------------------------------------
    Author: Sajjad Arif Gul
    Author URI: https://sajjadgul.com/
---------------------------------------------------------  */
var mob_main_nav;
var mob_cats_nav;

$(document).ready(function () {
    if (darkModeEnabled) {
        applyDarkMode();
    }

    //update the price and total items displayed in cart section on page 
    updateCartInfo();

    //apply cart popup fetching
    $("#cart-info").mouseenter(fetchCartItems).mouseleave(stopfetchCartItems);

    //apply language dropdown change
    var oDropdown = $(".language_drop").msDropdown({ roundedBorder: false }).data("dd");

    if (oDropdown) {
        oDropdown.on("change", function (res) {
            var redirectURL = $(this).val();

            if (redirectURL) {
                showSiteLoader();

                window.location.href = redirectURL;
            }
        });
    }

    //$(".language_drop").msDropdown({ roundedBorder: false });
    //$("#ddlanguages").change(function () {
    //    var languageHasResources = $(this).attr("[data-hasResources]");

    //    if (languageHasResources) {
    //        var redirectURL = $(this).val();

    //        if (redirectURL) {
    //            showSiteLoader();

    //            window.location.href = redirectURL;
    //        }
    //    }
    //    else {
    //        Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
    //    }
    //});

    //product picture click takes to product details
    $(".pi-pic").click(function () {
        var detailsLink = $(this).attr("data-href");

        if (detailsLink) {
            window.location.href = detailsLink;
        }
    });

    applyQuantityFunctions();

    $(".btnAddToCart").click(function (e) {
        var $btn = $(this);

        var $inputField = $btn.parents(".itemCartHolder").find("input.qtytxt");

        var quantity = 0;
        if ($inputField.length > 0) {
            quantity = $inputField.val();

            if (quantity === undefined || quantity === null || quantity === 0 || quantity < 0 || quantity > 1000) {
                quantity = 1;
                $inputField.val(quantity);
            }
        }
        else {
            quantity = 1;
        }

        Toast.fire({
            icon: 'info',
            iconHtml: '<i class="spinner-border spinner-border-sm"></i>',
            title: RESOURCE_ADDINGPRODUCTTOCART
        });

        $.ajax({
            url: ADDITEMTOCARTURL,
            method: 'post',
            data: {
                itemID: $btn.attr("data-id"),
                quantity: quantity
            }
        })
            .done(function (response) {
                if (response.Success) {
                    cartItems = response.CartItems;

                    updateCartInfo();

                    Toast.fire({
                        icon: 'success',
                        iconHtml: '<i class="fas fa-cart-plus"></i>',
                        title: response.Message
                    });
                }
                else {
                    Swal.fire(RESOURCE_ERRORHEADING, response.Message, "error");
                }
            })
            .fail(function () {
                Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
            });

        e.stopPropagation();
    });    


    $("#changeMode").click(function () {
        $.ajax({
            url: CHANGEMODEURL,
            method: 'post'
        })
        .done(function (response) {
            if (response.Success) {
                if (response.DarkModeEnabled === "true") {
                    applyDarkMode();
                }
                else {
                    clearDarkMode();
                }
            }
            else {
                Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
            }
        })
        .fail(function () {
            Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
        });
    });


    mob_main_nav = $(".mobile-menu").slicknav({
        label: RESOURCE_MENULABEL,
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true,
        'closedSymbol': RESOURCE_MENUICON,
        'beforeOpen': function (trigger) {
            mob_cats_nav.slicknav('close');
        }
    });

    mob_cats_nav = $("#cats").slicknav({
        label: RESOURCE_CATEGORIESLABEL,
        prependTo: '#categories-menu-wrap',
        allowParentLinks: true,
        'closedSymbol': RESOURCE_MENUICON,
        'beforeOpen': function (trigger) {
            mob_main_nav.slicknav('close');
        }
    });

    setTimeout(loadExternalSocialScripts, 3000);
});

//toast to be displayed
const Toast = Swal.mixin({
    toast: true,
    position: 'bottom',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    onOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer);
        toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
});

function getCartItems() {
    $.ajax({
        url: GETCARTITEMSURL,
        method: 'post'
    })
    .done(function (response) {
        if (response.Success) {
            cartItems = response.CartItems;

            updateCartInfo();
        }
    })
    .fail(function () {
        Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
    });
}

function updateCartInfo() {
    var cartTotalQuantity = sumOfArray(cartItems, 'Quantity');
    var cartTotalAmount = sumOfArray(cartItems, 'ProductTotal');

    if (!cartTotalQuantity) {
        cartTotalQuantity = 0;
    }

    if (!cartTotalAmount) {
        cartTotalAmount = 0;
    }

    $(".countholder", ".cartMenu").html(cartTotalQuantity);
    $(".cart-price", ".cartMenu").html(PRICECURRENCYPOSITION.replace('{price}', cartTotalAmount.toFixed(DIGITSAFTERDECIMAL)));
}

function validateNewsLetterForm() {
    $("#newsletter-form").validate({
        errorClass: "alert alert-danger mt-2 mb-0",
        errorElement: "div",
        rules: {
            email: {
                required: true,
                email: true
            }
        },
        messages: {
            email: {
                required: _newsLetterEmailRequired,
                email: _newsLetterEmailFormat
            }
        },
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        }
    });
}

function validateContactUsForm() {
    $("#contact-form").validate({
        errorClass: "alert alert-danger mb-0",
        errorElement: "div",
        rules: {
            subject: {
                required: true
            },
            name: {
                required: true
            },
            email: {
                required: true,
                email: true
            },
            message: {
                required: true
            }
        },
        messages: {
            subject: {
                required: _contactFormSubjectRequired
            },
            name: {
                required: _contactFormNameRequired
            },
            email: {
                required: _contactFormEmailRequired,
                email: _contactFormEmailFormat
            },
            message: {
                required: _contactFormMessageRequired
            }
        },
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        }
    });
}

function validateLoginForm() {
    $("#loginForm").validate({
        errorClass: "alert alert-danger mb-0",
        errorElement: "div",
        rules: {
            Username: {
                required: true
            },
            Password: {
                required: true
            }
        },
        messages: {
            Username: {
                required: _usernameRequired
            },
            Password: {
                required: _passwordRequired
            }
        },
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        }
    });
}

function validateRegisterForm() {
    $("#registerForm").validate({
        errorClass: "alert alert-danger mb-0",
        errorElement: "div",
        rules: {
            FullName: {
                required: true
            },
            Username: {
                required: true
            },
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#regPassword"
            }
        },
        messages: {
            FullName: {
                required: _fullNameRequired
            },
            Username: {
                required: _usernameRequired
            },
            Email: {
                required: _emailRequired,
                email: _emailFormat
            },
            Password: {
                required: _passwordRequired
            },
            ConfirmPassword: {
                required: _confirmPasswordRequired,
                equalTo: _passwordNotMatch
            }
        },
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        }
    });
}

function validateForgotPasswordForm() {
    $("#forgotPasswordForm").validate({
        errorClass: "alert alert-danger mb-0",
        errorElement: "div",
        rules: {
            Username: {
                required: true
            }
        },
        messages: {
            Username: {
                required: _usernameRequired
            }
        },
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        }
    });
}

function validateResetPasswordForm() {
    $("#resetPasswordForm").validate({
        errorClass: "alert alert-danger mb-0",
        errorElement: "div",
        rules: {
            Password: {
                required: true
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#regPassword"
            }
        },
        messages: {
            Password: {
                required: _passwordRequired
            },
            ConfirmPassword: {
                required: _confirmPasswordRequired,
                equalTo: _passwordNotMatch
            }
        },
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        }
    });
}

var searchTimeout;
function Search() {
    clearTimeout(searchTimeout);

    var searchURL = $("#searchURL").val();
    if (searchURL) {
        searchTimeout = setTimeout(function () {
            var url = searchURL;
            var priceMin = $("[name=from]").val();
            var priceMax = $("[name=to]").val();
            var sortBy = $("[name=sortBy]:enabled").val();
            var recordSize = $("[name=recordSize]:enabled").val();

            if (priceMin) {
                url = url + (!url.includes("?") ? "?" : "&") + "from=" + priceMin;
            }
            if (priceMax) {
                url = url + (!url.includes("?") ? "?" : "&") + "to=" + priceMax;
            }
            if (sortBy) {
                url = url + (!url.includes("?") ? "?" : "&") + "sortby=" + sortBy;
            }
            if (recordSize) {
                url = url + (!url.includes("?") ? "?" : "&") + "recordsize=" + recordSize;
            }

            window.location.href = url;
        }, 100);
    }
}

var fetchTimeout;
function fetchCartItems() {
    clearTimeout(fetchTimeout);

    if (CARTFETCHURL) {
        $(".bloader", ".cartMenu").show();
        $(".cart-items-modal", ".cartMenu").html("");

        fetchTimeout = setTimeout(function () {
            $.ajax({
                url: CARTFETCHURL
            })
            .done(function (response) {
                    $(".bloader", ".cartMenu").hide();
                    $(".cart-items-modal", ".cartMenu").html(response);
            })
            .fail(function () {
                Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
            });
        }, 1000);
    }
}

function stopfetchCartItems() {
    clearTimeout(fetchTimeout);
}

function sumOfArray(array, parameter) {
    let sum = null;
    if (array && array.length > 0 && typeof parameter === 'string') {
        sum = array.map(item => item[parameter])
            .reduce((prev, curr) => prev + curr, 0);
    }
    return sum;
}

function addLoader(containerID) {
    $("<div class='loading-overlay'><div class='spinner-div d-flex justify-content-center'><div class='align-self-center spinner-border' role='status'><span class='sr-only'>Loading...</span></div></div></div>").appendTo($("#" + containerID).css("position", "relative"));
}

function removeLoader(containerID) {
    $(".loading-overlay", "#" + containerID).remove();
}

function showSiteLoader() {
    $(".loader").fadeIn();
    $("#preloder").delay(200).fadeIn("slow");
}

function hideSiteLoader() {
    $(".loader").fadeOut();
    $("#preloder").delay(200).fadeOut("slow");
}

function applyQuantityFunctions() {
    $('.qtybtn').off('click').on('click', function () {
        var $button = $(this);
        var $input = $button.parent().find('.qtytxt');
        var oldValue = $input.val();
        var newVal = 0;

        if ($button.hasClass('inc')) {
            newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below one
            if (oldValue > 1) {
                newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }

        $input.val(newVal);
    });

    $('.qtytxt').off('keyup').on('keyup', function () {

        var vlue = parseInt($(this).val());

        if (Number.isInteger(vlue)) {
            if (vlue > 0 && vlue <= 1000) {
                $(this).val(vlue);
            }
            else {
                $(this).val(1);
            }
        }
        else {
            $(this).val(1);
        }
    });
}

function applyDarkMode() {
    if ($(".darkreader")[0]) {
        console.log("Darkreader Extension detected");
    } else {
        DarkReader.setFetchMethod(window.fetch);
        DarkReader.enable({
            brightness: 100,
            contrast: 90,
            sepia: 10
        });
    }
}

function clearDarkMode() {
    DarkReader.disable();
}

function loadExternalSocialScripts() {
    $.ajax({
        url: EXTERNALSOCIALSCRIPTSURL
    })
    .done(function (response) {
        $("#google-facebook-scripts").html(response);
    })
    .fail(function () {
        console.log("error loading ExternalSocialScripts");
    });
}