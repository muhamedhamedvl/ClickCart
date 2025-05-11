//$(document).ready(function () {
//    //update the price and total items displayed in cart section on page 
//    updateCartInfo();

//    //apply cart popup fetching
//    $("#cart-info").mouseenter(fetchCartItems).mouseleave(stopfetchCartItems);

//    //apply language dropdown change
//    $(".language_drop").msDropdown({ roundedBorder: false });
//    $("#ddlanguages").change(function () {
//        var redirectURL = $(this).val();

//        if (redirectURL) {
//            showSiteLoader();

//            window.location.href = redirectURL;
//        }
//    });

//    //product picture click takes to product details
//    $(".pi-pic").click(function () {
//        var detailsLink = $(this).attr("data-href");

//        if (detailsLink) {
//            window.location.href = detailsLink;
//        }
//    });

//    applyQuantityFunctions();

//    $(".btnAddToCart").click(function (e) {
//        var $btn = $(this);

//        var $inputField = $btn.parents(".itemCartHolder").find("input.qtytxt");

//        var quantity = 0;
//        if ($inputField.length > 0) {
//            quantity = $inputField.val();

//            if (quantity === undefined || quantity === null || quantity === 0 || quantity < 0 || quantity > 1000) {
//                quantity = 1;
//                $inputField.val(quantity);
//            }
//        }
//        else {
//            quantity = 1;
//        }

//        Toast.fire({
//            icon: 'info',
//            iconHtml: '<i class="spinner-border spinner-border-sm"></i>',
//            title: RESOURCE_ADDINGPRODUCTTOCART
//        });

//        $.ajax({
//            url: ADDITEMTOCARTURL,
//            method: 'post',
//            data: {
//                itemID: $btn.attr("data-id"),
//                quantity: quantity
//            }
//        })
//        .done(function (response) {
//            if (response.Success) {
//                cartItems = response.CartItems;

//                updateCartInfo();

//                Toast.fire({
//                    icon: 'success',
//                    iconHtml: '<i class="fas fa-cart-plus"></i>',
//                    title: response.Message
//                });
//            }
//            else {
//                Swal.fire(RESOURCE_ERRORHEADING, response.Message, "error");
//            }
//        })
//        .fail(function () {
//            Swal.fire(RESOURCE_ERRORHEADING, RESOURCE_GENERICERRORMESSAGE, "error");
//        });

//        e.stopPropagation();
//    });
//});