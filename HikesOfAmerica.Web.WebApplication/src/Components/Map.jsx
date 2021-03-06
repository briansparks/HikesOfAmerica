import React, { Component } from "react";
import { Map as LeafletMap, TileLayer, Marker, Popup } from 'react-leaflet';
import Lightbox from 'react-image-lightbox';
import 'react-image-lightbox/style.css';

const initialCentering = [36.082,-95.933];

export default class Map extends Component {
    constructor(props) {
        super(props);

        this.state = { "fullScreen" : false, "expandedImageSource" : ""};
    }

    render() {
        const markers = [];
        const { fullScreen, expandedImageSource } = this.state;
    
        if (this.props.locations !== null && this.props.locations !== undefined && this.props.locations.length !== 0) {
            this.props.locations.forEach(function (location) {
                var pos = [location.longitude, location.latitude];
    
                var trails = location.trails.map((trail, index) => {
                    return <div id="trailInfo" key={index}>
                            <p className="trailsDetails"><b>Name: </b>{trail.name}</p>
                            <p className="trailsDetails"><b>Distance: </b>{trail.distance} mi</p>
                        </div>
                });
    
                var images = location.images.map(function(imageUrl, index) {
                    return <img id="locationImage" key={index} src={imageUrl} onClick={() => this.setState({ fullScreen: true, expandedImageSource : imageUrl })} />
                }, this);
    
                markers.push(
                    <Marker position={pos} key={pos}> 
                        <Popup>
                            <h1 id="hikeTitle">{location.name}</h1>
                            <p>{location.description}</p>
                            <div>
                                <h2 className="subHeader">Trails</h2>
                                <div id="trailsBody">
                                    {trails}
                                </div>
                                <h2 className="subHeader">Photos</h2>
                                <div id="imagesBody">
                                    {images}
                                </div>
                            </div>
                        </Popup>
                    </Marker>
                )
            }, this)
        }
    
        if (this.props.viewingCoordinates !== null && this.props.viewingCoordinates !== undefined && this.props.viewingCoordinates !== "") { 
            return <div id="map">
                <LeafletMap
                    center={this.props.viewingCoordinates}
                    zoom={this.props.zoomLevel}
                    maxZoom={11}
                    attributionControl={true}
                    zoomControl={true}
                    doubleClickZoom={true}
                    scrollWheelZoom={true}
                    dragging={true}
                    animate={true}
                    easeLinearity={0.35}
                >
                    <TileLayer
                    url='https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png'
                    attribution='Map data: &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, <a href="http://viewfinderpanoramas.org">SRTM</a> | Map style: &copy; <a href="https://opentopomap.org">OpenTopoMap</a> (<a href="https://creativecommons.org/licenses/by-sa/3.0/">CC-BY-SA</a>)'
                    />
                    {markers}
                    {fullScreen && (
                        <Lightbox
                            mainSrc={expandedImageSource}
                            onCloseRequest={() => this.setState({ fullScreen: false })}
                        />
                    )}
                </LeafletMap>
            </div>
        }
        else {   
            return <div id="map">
                <LeafletMap
                    center={initialCentering}
                    zoom={this.props.zoomLevel}
                    maxZoom={11}
                    attributionControl={true}
                    zoomControl={true}
                    doubleClickZoom={true}
                    scrollWheelZoom={true}
                    dragging={true}
                    animate={true}
                    easeLinearity={0.35}
                >
                    <TileLayer
                    url='https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png'
                    // url='http://{s}.tile.osm.org/{z}/{x}/{y}.png'
                    attribution='Map data: &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, <a href="http://viewfinderpanoramas.org">SRTM</a> | Map style: &copy; <a href="https://opentopomap.org">OpenTopoMap</a> (<a href="https://creativecommons.org/licenses/by-sa/3.0/">CC-BY-SA</a>)'
                    />
                    {markers}
                    {fullScreen && (
                        <Lightbox
                            mainSrc={expandedImageSource}
                            onCloseRequest={() => this.setState({ fullScreen: false })}
                        />
                    )}
                </LeafletMap>
            </div>
        }
    }
}

