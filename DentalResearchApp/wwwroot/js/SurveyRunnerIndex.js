function getParams() {
    var url = window.location.href
        .slice(window.location.href.indexOf("?") + 1)
        .split("&");
    var result = {};
    url.forEach(function (item) {
        var param = item.split("=");
        result[param[0]] = param[1];
    });
    return result;
}

function verifyLink() {

    var linkId = decodeURI(getParams()["id"]);
    var asdf = {};
    asdf.LinkId = linkId;

    $.ajax({
        url: "/Cookie/VerifyLinkId",
        dataType: "json",
        type: "post",
        contentType: "application/json",
        data: JSON.stringify(asdf),
        success: function (data) {
            window.location.href = "/surveyrunner?id=" + data.message;
          // alert(data.message);
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}