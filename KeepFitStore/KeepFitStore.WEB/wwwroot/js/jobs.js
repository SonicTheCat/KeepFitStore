function showApplicationDetails() {
    $(".show-more-details").click(function () {

        let btn = $(this);
        let detailsDiv = btn.parent().next();
        let bio = btn.prev();

        if (detailsDiv.attr("opened") === "false") {
            detailsDiv.attr("opened", "true");
            btn.text("Hide details");
            btn.addClass("btn-danger");
            bio.hide("slow");
        }
        else {
            detailsDiv.attr("opened", "false");
            btn.text("See more");
            btn.removeClass("btn-danger");
            bio.show("slow");
        }

        detailsDiv.toggle("slow");
    })
}

function toggleApplyForJobForm() {
    $("#show-job-form-btn").click(function () {
        $("#job-form").toggle("slow");
    });
}