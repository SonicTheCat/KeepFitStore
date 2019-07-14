function submitReviewForm() {
    $("form").on("submit", function (evt) {
        evt.preventDefault();
        var title = $("#title").val();
        var rating = $("#rating").val();
        var content = $("#content").val();
        var productId = $("textarea").attr("productId");

        let inputModel = {
            Title: title,
            GivenRating: rating,
            Content: content,
            ProductId: productId
        };

        var antiForgery = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            type: "POST",
            url: "/api/ReviewsApi/Create",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(inputModel),
            beforeSend: function (request) {
                request.setRequestHeader("RequestVerificationToken", antiForgery);
            },
            success: function (data) {
                location.reload();
            },
            error: function (err) {
                let errorDiv = $("#error-message");
                let errorAsJson = err.responseJSON.errors;
                errorDiv
                    .empty()
                    .show()
                    .append("<p>All Lines are required!</p>");

                if (err.status === 400) {
                    if (errorAsJson.GivenRating !== undefined) {
                        errorDiv.append("<p>Rating value must be between 1 and 5</p>");

                    } else {
                        for (let e in errorAsJson) {
                            errorDiv.append(`<p>${errorAsJson[e][0]}</p>`);
                        }
                    }
                }
            }
        });
    });
}

function goToReviewsForCurrentPrdocut() {
    $("#rating-stars").on("click", function () {
        $("#nav-reviews-tab").click();

        setTimeout(function () {
            $('html, body').animate({
                scrollTop: ($('#nav-reviews').offset().top)
            }, 800)
        }, 200);
    });
}