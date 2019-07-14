const addressForm = $("#address-form");
const addressDiv = $("#address-div");
const billingDiv = $("#billing-div")

function addAddress() {
    addressForm.submit(function (evt) {
        evt.preventDefault();

        let isFormValid = addressForm.valid();
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
                addressDiv.toggle("slow");
                billingDiv.toggle("slow");
            },
            error: function (er) {
                console.log(er)
            }
        });
    });
}