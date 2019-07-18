const ADDRESS_FORM = $("#address-form");
const WELCOME_FORM = $("#welcome-form");

const WELCOME_DIV = $("#welcome-div");
const ADDRESS_DIV = $("#address-div");
const BILLING_DIV = $("#billing-div");

const INPUT_DELIVERY_TYPE_RADIO_BTN = $('input[type=radio][name=deliveryType]');
const TOTAL_SUM = $("#totalSum");
const EDIT_BUTTON = $("#edit-btn");

const EXPRESS_DELIVERY = "Express";
const NEXTDAY_DELIVERY = "NextDay";
const STANDART_DELIVERY = "Standart";

const EXPRESS_PRICE = 15;
const NEXTDAY_PRICE = 10;
const STANDART_PRICE = 5;

const DETAILS_ORDER_BTN = $(".details-order-btn");

function addUsersInfo() {
    WELCOME_FORM.submit(function (evt) {
        evt.preventDefault();

        let isFormValid = ADDRESS_FORM.valid();
        if (!isFormValid) {
            return;
        }

        let data = {
            FullName: $("#User_FullName").val(),
            PhoneNumber: $("#User_PhoneNumber").val(),
        };

        var antiForgery = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            type: "POST",
            url: "/api/UserApi/Update",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            beforeSend: function (request) {
                request.setRequestHeader("RequestVerificationToken", antiForgery);
            },
            success: function (data) {
                toggleElements(WELCOME_DIV, ADDRESS_DIV);

                $("#confirmation-data")
                    .append($("<ul>")
                        .append($(`<li>`)
                            .append($(`<p>Full name:<em> ${data.fullName}</em></p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Mobile:<em> ${data.phoneNumber}</em></p>`).addClass("h6"))));
            },
            error: function (er) {
                console.log(er)
            }
        });
    });
}

function addAddress() {
    ADDRESS_FORM.submit(function (evt) {
        evt.preventDefault();

        let isFormValid = ADDRESS_FORM.valid();
        if (!isFormValid) {
            return;
        }

        let data = {
            CityName: $("#DeliveryAddress_City_Name").val(),
            PostCode: $("#DeliveryAddress_City_PostCode").val(),
            StreetName: $("#DeliveryAddress_StreetName").val(),
            StreetNumber: $("#DeliveryAddress_StreetNumber").val()
        };

        console.log(data);
        var antiForgery = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            type: "POST",
            url: "/api/AddressApi/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
            beforeSend: function (request) {
                request.setRequestHeader("RequestVerificationToken", antiForgery);
            },
            success: function (data) {
                toggleElements(ADDRESS_DIV, BILLING_DIV);

                $("#confirmation-data")
                    .append($("<ul>")
                        .append($(`<li>`)
                            .append($(`<p>City:<em> ${data.cityName}</em></p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Post code:<em> ${data.cityPostCode}</em></p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Street name:<em> ${data.streetName}</em></p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Street number:<em> ${data.streetNumber}</em></p>`).addClass("h6"))));
            },
            error: function (er) {
                console.log(er)
            }
        });
    });
}

function chooseDeliveryType() {
    INPUT_DELIVERY_TYPE_RADIO_BTN.change(function () {

        let chosenType = this.value;
        let totalSum = Number(TOTAL_SUM.attr("total-sum"));

        if ((chosenType !== EXPRESS_DELIVERY && totalSum >= 60) ||
            (chosenType === STANDART_DELIVERY && totalSum >= 20)) {

            TOTAL_SUM.text(`Total: £${totalSum.toFixed(2)}`);
            return;
        }

        let deliveryPrice = 0;
        let message = "";
        if (chosenType === EXPRESS_DELIVERY) {
            deliveryPrice = EXPRESS_PRICE;
            message = "Express delivery: £" + deliveryPrice;
        } else if (chosenType === NEXTDAY_DELIVERY) {
            deliveryPrice = NEXTDAY_PRICE;
            message = "Next day delivery: £" + deliveryPrice;
        } else {
            deliveryPrice = STANDART_PRICE;
            message = "Standart delivery: £" + deliveryPrice;
        }

        TOTAL_SUM
            .text(`Total (plus delivery): £${(totalSum + deliveryPrice).toFixed(2)}`)
            .prepend(`<p>${message}</p>`);
    });
}

function toggleElements(elementOne, elementTwo) {
    elementOne.toggle("slow");
    elementTwo.toggle("slow");
}

function startFromBeginning() {
    EDIT_BUTTON.click(function () {
        location.reload();
    });
}

function openDetailsForOrder() {
    DETAILS_ORDER_BTN.click(function (evt) {
        evt.preventDefault();

        var btn = $(this);
        let orderId = btn.parent().parent().children(':first-child').text();
        let rowDetailsToBeShown = btn.parent().parent().next();
        var detailsAttribute = rowDetailsToBeShown.attr("details");

        if (detailsAttribute === "opened") {
            rowDetailsToBeShown.attr("details", "closed");
        } else {
            if (detailsAttribute === undefined) {
                let elementToAppendDataTo = rowDetailsToBeShown.children(':first-child');
                elementToAppendDataTo.load(`/Orders/Details?orderId=${orderId}`)
            }
            rowDetailsToBeShown.attr("details", "opened");
        }

        rowDetailsToBeShown.toggle();
    });
}