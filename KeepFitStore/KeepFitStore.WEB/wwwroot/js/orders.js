const ADDRESS_FORM = $("#address-form");
const ADDRESS_DIV = $("#address-div");
const BILLING_DIV = $("#billing-div")

const INPUT_DELIVERY_TYPE_RADIO_BTN = $('input[type=radio][name=deliveryType]');
const TOTAL_SUM = $("#totalSum");
const EXPRESS_DELIVERY = "Express";
const NEXTDAY_DELIVERY = "NextDay";
const STANDART_DELIVERY = "Standart";
const EXPRESS_PRICE = 15;
const NEXTDAY_PRICE = 10;
const STANDART_PRICE = 5;

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
                console.log(data);
                $("#confirmation-data")
                    .append($("<ul>")
                        .append($(`<li>`)
                            .append($(`<p>City: ${data.cityName}</p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Post code: ${data.cityPostCode}</p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Street name: ${data.streetName}</p>`).addClass("h6")))
                        .append($(`<li>`)
                            .append($(`<p>Street number: ${data.streetNumber}</p>`).addClass("h6"))));


                ADDRESS_DIV.toggle("slow");
                BILLING_DIV.toggle("slow");
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