mapboxgl.accessToken = 'pk.eyJ1IjoiZzFnYXNldCIsImEiOiJjbDlyamMyOW0wand4NDFtazE3OW1oM2pzIn0.sdKz6G3KjyyK1k6Kvaw09A';

let map;

function initMap()
{
    // Инициализирует карту
    map = new mapboxgl.Map({
        container: 'map', // container ID
        // style: 'mapbox://styles/mapbox/streets-v11', // style URL
        style: 'mapbox://styles/mapbox/light-v10', // style URL
        center: [37.617698, 55.755864], // starting position [lng, lat]
        zoom: 10, // starting zoom
        projection: 'mercator', // display the map as a 3D globe
        language: 'ru'
    });

    // Добавляет управление
    map.addControl(new mapboxgl.NavigationControl());

    // Выбор языка карты
    map.addControl(new MapboxLanguage({
        defaultLanguage: 'ru'
    }));
    
    let attrib = document.getElementsByClassName("mapboxgl-ctrl-attrib-inner")[0];
    let bottom = document.getElementsByClassName("mapboxgl-ctrl-bottom-left")[0];
    attrib.id = "turn-off-signatures1";
    bottom.id = "turn-off-signatures2";
    attrib = document.getElementById("turn-off-signatures1");
    attrib.style.display = 'none';
    bottom = document.getElementById("turn-off-signatures2");
    bottom.style.display = 'none';
}

function addTerminals(source)
{
    let sourceName = "terminals";
    
    map.on('load', () => {
        // добавляем данные точек
        map.addSource(sourceName, source);

        // отображаем и задаем иконку
        map.addLayer({
            'id': "terminal-icon",
            'type': 'symbol',
            'source': sourceName,
            'filter': ['!', ['has', 'point_count']],
            'layout': {
                'icon-image': '{icon}',
                'icon-allow-overlap': true,
                'icon-size': 0.9,
                'icon-anchor': 'bottom'
            }
        });

        // добавляем группировку на точки
        map.addLayer({
            id: "terminal-cluster",
            type: 'circle',
            source: sourceName,
            filter: ['has', 'point_count'],
            paint: {
                'circle-color': [
                    'step',
                    ['get', 'point_count'], 
                    'rgb(64,102,233)', 1,
                    'rgb(64,102,233)'
                ],
                'circle-radius': [
                    'step',
                    ['get', 'point_count'],
                    20,
                    100,
                    30,
                    750,
                    40
                ],
                'circle-stroke-color': "#fff",
                'circle-stroke-width': 2
            }
        });
        
        // добавляем отображение кол-ва сгруппированных точек
        map.addLayer({
            id: "terminal-cluster-count",
            type: 'symbol',
            source: sourceName,
            filter: ['has', 'point_count'],
            layout: {
                'text-field': '{point_count_abbreviated}',
                'text-font': ['Montserrat Bold'],
                'text-size': 15,
                // 'text-offset': [0, 1.25],
            },
            paint: {
                'text-color': "#fff"
            }
        });
        
        // добавляем ивент на отображение информации
        map.on('click', 'terminal-icon', initPopup);
    });
}

function initPopup(event)
{
    const features = map.queryRenderedFeatures(event.point, {
        layers: ['terminal-icon']
    });
    if (!features.length) {
        return;
    }
    const feature = features[0];

    const popup = new mapboxgl.Popup({ offset: [0, -15] })
        .setLngLat(feature.geometry.coordinates)
        .setHTML(`
    <p class="text-meta">Терминал: ${feature.properties.id}</p>
    `).addTo(map);
}

function initInfoPopup(event)
{
    const features = map.queryRenderedFeatures(event.point, {
        layers: ['terminal-icon']
    });
    if (!features.length) {
        return;
    }
    const feature = features[0];

    const popup = new mapboxgl.Popup({ offset: [0, -15] })
        .setLngLat(feature.geometry.coordinates)
        .setHTML(`
    <p class="text-meta">Терминал: ${feature.properties.id}</p>
    <p class="text-meta">Сумма: ${feature.properties.money}</p>
    <p class="text-meta">Дата: ${feature.properties.date}</p>
    `).addTo(map);
}

function addTerminalsInfo(source)
{
    let layerNameIcon = "terminal-icon";
    let layerNameCluster = "terminal-cluster";
    let layerNameClusterCount = "terminal-cluster-count";
    
    // добавляем точки с информацией
    let sourceName = 'terminals-info';
    // добавляем данные точек
    map.addSource(sourceName, source);

    // отображаем и задаем иконку
    map.addLayer({
        'id': layerNameIcon,
        'type': 'symbol',
        'source': sourceName,
        'filter': ['!', ['has', 'point_count']],
        'layout': {
            'icon-image': '{icon}',
            'icon-allow-overlap': true,
            'icon-size': 0.9,
            'icon-anchor': 'bottom'
        }
    });

    // добавляем группировку на точки
    map.addLayer({
        id: layerNameCluster,
        type: 'circle',
        source: sourceName,
        filter: ['has', 'point_count'],
        paint: {
            'circle-color': [
                'step',
                ['get', 'point_count'],
                'rgb(64,102,233)', 1,
                'rgb(64,102,233)'
            ],
            'circle-radius': [
                'step',
                ['get', 'point_count'],
                20,
                100,
                30,
                750,
                40
            ],
            'circle-stroke-color': "#fff",
            'circle-stroke-width': 2
        }
    });

    // добавляем отображение кол-ва сгруппированных точек
    map.addLayer({
        id: layerNameClusterCount,
        type: 'symbol',
        source: sourceName,
        filter: ['has', 'point_count'],
        layout: {
            'text-field': '{point_count_abbreviated}',
            'text-font': ['Montserrat Bold'],
            'text-size': 15,
            // 'text-offset': [0, 1.25],
        },
        paint: {
            'text-color': "#fff"
        }
    });

    // добавляем ивент на отображение информации
    map.on('click','terminal-icon',initInfoPopup);
}

function removeOldTerminalMoney()
{
    let layerNameIcon = "terminal-icon";
    let layerNameCluster = "terminal-cluster";
    let layerNameClusterCount = "terminal-cluster-count";

    let sourceName = 'terminals-info';

    // убираем ивент со старого слоя
    map.off('click', 'terminal-icon', initInfoPopup);
    // удаляем начальный слой точек
    map.removeLayer(layerNameIcon);
    map.removeLayer(layerNameCluster);
    map.removeLayer(layerNameClusterCount);
    // удаляем источник информации прошлых точек
    map.removeSource(sourceName);
}

function removeInitTerminal()
{
    let layerNameIcon = "terminal-icon";
    let layerNameCluster = "terminal-cluster";
    let layerNameClusterCount = "terminal-cluster-count";

    // убираем ивент со старого слоя
    map.off('click', 'terminal-icon', initPopup);
    // удаляем начальный слой точек
    map.removeLayer(layerNameIcon);
    map.removeLayer(layerNameCluster);
    map.removeLayer(layerNameClusterCount);
    // удаляем источник информации прошлых точек
    map.removeSource('terminals');
}

function addTestRoutes(source)
{
    map.on('load', () => {
        // добавляем данные линии
        map.addSource("routes", source);

        // отображаем линии
        map.addLayer({
            'id': "routes-line",
            'type': 'line',
            'source': "routes",
            'layout': {
                'line-join': 'round',
                'line-cap': 'round'
            },
            'paint': {
                'line-color': '#ff0000',
                'line-width': 5
            }
        });
        
        map.moveLayer('routes-line', 'terminal-icon');
    });
}

function addRoute(source, carId, color)
{
    let sourceId = `route-${carId}`
    let layerId = `route-line-${carId}`
    // добавляем данные линии
    map.addSource(sourceId, source);

    // отображаем линии
    map.addLayer({
        'id': layerId,
        'type': 'line',
        'source': sourceId,
        'layout': {
            'line-join': 'round',
            'line-cap': 'butt'
        },
        'paint': {
            'line-color': ["get", "color"],
            'line-width': 5,
            'line-opacity': 0.5
        }
    });

    map.moveLayer(layerId, 'terminal-icon');
}

function removeOldRoute(carId)
{
    let sourceId = `route-${carId}`
    let layerId = `route-line-${carId}`

    // удаляем слой линий
    map.removeLayer(layerId);
    // удаляем источник информации
    map.removeSource(sourceId);
}

function noneVisibleTerminal()
{
    let layerNameIcon = "terminal-icon";
    let layerNameCluster = "terminal-cluster";
    let layerNameClusterCount = "terminal-cluster-count";
    
    map.setLayoutProperty(layerNameIcon, 'visibility', 'none');
    map.setLayoutProperty(layerNameCluster, 'visibility', 'none');
    map.setLayoutProperty(layerNameClusterCount, 'visibility', 'none');
}

function visibleTerminal()
{
    let layerNameIcon = "terminal-icon";
    let layerNameCluster = "terminal-cluster";
    let layerNameClusterCount = "terminal-cluster-count";

    map.setLayoutProperty(layerNameIcon, 'visibility', 'visible');
    map.setLayoutProperty(layerNameCluster, 'visibility', 'visible');
    map.setLayoutProperty(layerNameClusterCount, 'visibility', 'visible');
}

function styleCarRouteBtn(carId)
{
    let layerId = `route-line-${carId}`
    var doc = document.getElementById(`car-route-${carId}`);
    
    if (doc.classList.contains("setting-btn-active"))
    {
        doc.classList.remove("setting-btn-active");
        map.setLayoutProperty(layerId, 'visibility', 'none');
    }
    else
    {
        doc.classList.add("setting-btn-active");
        map.setLayoutProperty(layerId, 'visibility', 'visible');
    }
}

function uploadIcons()
{
    map.loadImage(`img/Terminals2.png`, (error, image) => {
        if (error) throw error;
        map.addImage('terminal', image);
    });
}