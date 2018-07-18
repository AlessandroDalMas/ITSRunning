$("#start").click(startRecording);
$("#stop").click(endRecording);

var myTimer;
var startPoint;
var lines = [];
var map;
var telemetryEndpoint = "/Runner/SendTelemetry";
var apiKey = "STu3rHGlE02Gr-ijZXWPPiVNLjKi7HT2hTllZCHwACo";

function startRecording() {
    $("#start").addClass("disabled");
    $("#stop").removeClass("disabled");
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(initMap);
    }
}

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(useLocation);
    }
};

function useLocation(position) {
    //to change position
    var longitude = position.coords.longitude + (Math.random() / 100);
    var latitude = position.coords.latitude + (Math.random() / 100);

    lines.push([longitude, latitude]);
    /* Create a line*/
    var line = new atlas.data.Feature(
        new atlas.data.LineString(
            lines
        ),
        {
            width: 4,
            color: "orange",
        }
    );

    /* Add the line to the map*/
    map.addLinestrings([line], {
        name: "line-layer"
    });

    console.log(line);

    var instant = new Date().toISOString();

    var data = {
        "Longitude": longitude,
        "Latitude": latitude,
        "Instant": instant,
        "IdActivity": idActivity
    }

    fetch(telemetryEndpoint, {
        body: JSON.stringify(data),
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        credentials: 'include',
        method: 'POST'
    });
};

function initMap(position) {
    /* Instantiate map to the div with id "map" */
    startPoint = [position.coords.longitude, position.coords.latitude];
    lines.push(startPoint);
    map = new atlas.Map("map", {
        "subscription-key": apiKey,
        center: startPoint,
        zoom: 12
    });
    map.addLinestrings([], {
        name: "line-layer",
        cap: "butt",
        join: "round"
    });
    myTimer = setInterval(getLocation, 5000);
    $('#cameraLauncher').show();
}

function endRecording() {
    $("#start").removeClass("disabled");
    $("#stop").addClass("disabled");
    if (myTimer)
        clearTimeout(myTimer);
}

function getAddressFromLatLon() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(useLocation);
    }
    function useLocation(position) {
        var endPoint = "https://atlas.microsoft.com/search/address/reverse/json?api-version=1.0&subscription-key=";
        var params = "&query=" + position.coords.latitude + "," + position.coords.longitude;
        var url = endPoint + apiKey + params;

        fetch(url).then(function (response) {
            return response.json();
        }).then(function (myAddress) {
            var address = myAddress.addresses[0].address.freeformAddress;
            $("input[name='Location']").val(address);
        });
    };
}

function drawActivity(telemetries) {
    lines = [];
    startPoint = [telemetries[0].Longitude, telemetries[0].Latitude];
    lines.push(startPoint);

    map = new atlas.Map("map", {
        "subscription-key": apiKey,
        center: startPoint,
        zoom: 12
    });

    map.addLinestrings([], {
        name: "line-layer",
        cap: "butt",
        join: "round"
    });


    setTimeout(function () {
        telemetries.shift();
        for (var telemetry of telemetries) {
            lines.push([telemetry.Longitude, telemetry.Latitude]);

            if (telemetry.UriSelfie) {
                var selfie = document.createElement('div');
                selfie.classList.add("ms-pin");
                selfie.setAttribute("style", "background-image: url(" + telemetry.UriSelfie + ");")
                var selfieLocation = [telemetry.Longitude, telemetry.Latitude];
                map.addHtml(selfie, selfieLocation);
            }
        }
        /* Create the line*/
        var line = new atlas.data.Feature(
            new atlas.data.LineString(
                lines
            ));

        /* Add the line to the map*/
        map.addLinestrings(line, {
            name: "xxx",
            color: "#2272B9",
            width: 5,
            cap: "round",
            join: "round"
        });

        console.log(line);
    }, 5000);
}