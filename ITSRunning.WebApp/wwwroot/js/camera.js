var camera;
var snapshot;

function hideButtons() {
    $("#reloadCamera").hide();
    $("#savePic").hide();
    $("#shotPic").hide();
}

function loadCamera() {
    camera = new JpegCamera("camera").ready(function () {
        $("#shotPic").show();
    });
}

function shotPic() {
    $("#shotPic").hide();
    $("#reloadCamera").show();
    $("#savePic").show();
    snapshot = camera.capture().show();
}

function reloadCamera() {
    $("#reloadCamera").hide();
    $("#savePic").hide();
    camera = new JpegCamera("camera").ready(function () {
        $("#shotPic").show();
    });
}

function savePic() {
    $("#reloadCamera").hide();
    $("#savePic").hide();
    $("#shotPic").show();

    //to be solved
    /*var test1 = snapshot.get_image_data((done) => {
        var arrayImage = Array.from(done.data);
        var data = {
            "Pic": arrayImage,
            "IdActivity": idActivity,
            "Instant": new Date().toISOString()
        };
        sendPic(data);
        reloadCamera();
    });*/

    var test2 = snapshot.get_canvas((done) => {
        var data = {
            "Pic": done.toDataURL('image/jpeg', 1.0),
            "IdActivity": idActivity,
            "Instant": new Date().toISOString()
        };
        sendPic(data);
        reloadCamera();
    });
}

function sendPic(blob) {
    console.log(blob);
    fetch(uriPicsEndPoint, {
        body: JSON.stringify(blob),
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        credentials: 'include',
        method: 'POST'
    });
}

$('#cameraModal').on('shown.bs.modal', loadCamera);
$('#cameraModal').on('show.bs.modal', hideButtons);
$('#shotPic').click(shotPic);
$('#reloadCamera').click(reloadCamera);
$('#savePic').click(savePic);
window.onresize = reloadCamera;