﻿function disableButtonIfQuantityIsOne() {
    var inputQuantities = $("input[type='number']").toArray();
    for (var i in inputQuantities) {
        var currentInput = $(inputQuantities[i])
        var quantity = currentInput.val();
        if (quantity == 1) {
            var element = currentInput.prev().children().eq(0)[0];
            $(element).css("pointer-events", "none");
            $(element).css("cursor", "default");
        }
    }
}

function objectifyForm(formArray) {

    let returnArray = {};
    for (let i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}

//function attachQuantityClickEvent() {
//    let quantityButtons = $("button[dataType]");
//    quantityButtons.on("click", function () {

//        let pressedButton = $(this);

//        let buttonValue = pressedButton.attr("dataType");
//        let inputQuantity;
//        let newValue;

//        if (buttonValue == "plus") {
//            inputQuantity = pressedButton.parent().prev();
//            newValue = Number(inputQuantity.val()) + 1;
//            if (newValue === 2) {
//                let minusBtn = pressedButton.parent().prev().prev().find("button");
//                minusBtn.removeAttr("disabled")
//            }
//        } else if (buttonValue == "minus") {
//            inputQuantity = pressedButton.parent().next();
//            newValue = Number(inputQuantity.val()) - 1;
//            if (newValue === 1) {
//                pressedButton.attr("disabled", "disabled");
//            }
//        }

//        let hiddenInputWithIds = pressedButton.parent().parent().prev();
//        let basketId = hiddenInputWithIds.attr("basketId");
//        let productId = hiddenInputWithIds.attr("productId");

//        $.ajax({
//            type: "GET",
//            url: `api/BasketApi/EditQuantity?basketId=${basketId}&productId=${productId}&quantity=${newValue}`,
//            success: function (data) {
//                let elementTotalPrice = pressedButton
//                    .parent()
//                    .parent()
//                    .parent()
//                    .next()
//                    .find("[reb='price']");

//                let newTotalPrice = parseFloat(data.productPrice * data.quantity).toFixed(2)
//                elementTotalPrice.text(newTotalPrice);
//                inputQuantity.val(newValue);
//                recalculateTotalBasketValue();
//            }
//        });
//    });
//}

//function attachRemoveClickEvent() {
//    let removeButton = $("button.remove");
//    removeButton.on("click", function () {
//        let pressedBtn = $(this);
//        var hiddenInputWithIds = pressedBtn.parent().parent().find("input[type=hidden]");
//        let basketId = hiddenInputWithIds.attr("basketId");
//        let productId = hiddenInputWithIds.attr("productId");

//        $.ajax({
//            type: "GET",
//            url: `api/BasketApi/DeleteBasketItem?basketId=${basketId}&productId=${productId}`,
//            success: function (data) {
//                let container = pressedBtn.parent().parent().parent();
//                checkIfLastElementInBasket(container);
//                pressedBtn.parent().parent().remove();
//                recalculateTotalBasketValue();
//            }
//        });
//    });
//}

//function checkIfLastElementInBasket(container) {
//    let removeButtons = container.find($("button.remove"));
//    if (removeButtons.length <= 1) {
//        $.ajax({
//            type: "GET",
//            url: `/Basket/Index`,
//            success: function (data) {
//                var newDoc = document.open("text/html", "replace");
//                newDoc.write(data);
//                newDoc.close();
//            }
//        });
//    }
//}

//function recalculateTotalBasketValue() {
//    let totalBasketValueDiv = $("#totalBasketValue");
//    let container = totalBasketValueDiv.parent().parent();
//    let allBasketItemValues = container.find("[reb='price']");

//    let sum = 0.0;
//    allBasketItemValues.each(function (index) {
//        sum += parseFloat($(this).text());
//    });

//    totalBasketValueDiv.find("span").text(sum.toFixed(2));
//}
