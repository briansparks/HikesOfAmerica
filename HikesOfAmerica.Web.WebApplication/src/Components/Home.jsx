import React, { Component } from "react";
import Navbar from "./Navbar";
import Map from "./Map";
import ImageUploader from 'react-images-upload';
import axios from 'axios';

export default class Home extends Component {
    constructor (props) {
        super (props);

        this.state = { viewingState : "", locations : [], flattenedLocations : [], displayModal : false, uploadedImage : {}, imageUploadStatus : "notStarted", zoomLevel : 5};
    }

    componentDidMount() {
        fetch("https://localhost:44308/api/map/locations/")
        .then(x => x.json())
        .then((result) => {this.handleLocationsResult(result)})
        .catch(console.log);        
    }

    handleLocationsResult(locations) {
        var flattenedLocations = [];
        
        locations.forEach(function (location) {
            let coordinates = [ location.longitude, location.latitude ];
            flattenedLocations.push({ name: location.name, coordinates: coordinates })
            location.trails.forEach(function (trail) {
                flattenedLocations.push({ name: trail.name, coordinates: coordinates })
            });
        });

        this.setState({ locations: locations, flattenedLocations: flattenedLocations });
    }

    switchToCoordinatesView(viewingCoordinates, zoomLevel) {
        this.setState({viewingCoordinates : viewingCoordinates, zoomLevel : zoomLevel});
    }

    showModal() {
        this.setState({ displayModal: true });
    };
    
    handleSubmit(event) {
        event.preventDefault();

        var locationName = document.getElementById('location-name').value;
        var hikeName = document.getElementById('hike-name').value;
        var distance = document.getElementById('distance').value;
        var lat = document.getElementById('latitude').value;
        var lng = document.getElementById('longitude').value;

        var trails = [];
        var trail = {"Name" : hikeName, "Distance" : distance};
        trails.push(trail);

        const formData = new FormData();
        formData.append("name", locationName);
        formData.append("trails", JSON.stringify(trails));
        formData.append("longitude", lng);
        formData.append("latitude", lat);
        formData.append("description", "");
        formData.append("file", this.state.uploadedImage);

        const config = {
            headers: {
            'content-type': 'multipart/form-data'
            }
          }    
        
        axios.post(`https://localhost:44308/api/map/location/submit`, formData, config)
        .then(this.setState({ imageUploadStatus : "success" }))
        .catch(err => {console.log(err); this.setState({ imageUploadStatus : "failed" });});  
    };

    handleClose() {
        this.setState({ displayModal: false });
    };

    handleImageUpload(image) {
        this.setState({ uploadedImage: image});
    }

    render()
    {
        return (
            <div id="main">
                <Navbar 
                    switchToCoordinatesView={this.switchToCoordinatesView.bind(this)}
                    showModal={this.showModal.bind(this)}
                    flattenedLocations={this.state.flattenedLocations}
                />
                <SubmissionModal 
                    displayModal={this.state.displayModal} 
                    handleSubmit={this.handleSubmit.bind(this)}
                    handleClose={this.handleClose.bind(this)}
                    handleImageUpload={this.handleImageUpload.bind(this)} 
                />
                <Map 
                    locations={this.state.locations}
                    viewingCoordinates={this.state.viewingCoordinates}
                    zoomLevel={this.state.zoomLevel}
                />
            </div>
        );
    }
}

function SubmissionModal(props) {
    const showHideClassName = props.displayModal ? "modal display-block" : "modal display-none";
    
    return (
        <div className={showHideClassName}>
          <section class="modal-main">
          <button type="button" id="close" aria-label="Close" onClick={props.handleClose}>
            <span aria-hidden="true">&times;</span>
          </button>
          <form class="form-inline my-2 my-lg-0" onSubmit={props.handleSubmit}>
              <table class="submit-table">
                    <tbody>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Location Name: </b></p></td>
                            <td><input class="form-control mr-sm-2 form-input" id="location-name" type="text" placeholder="Name of the park or location..." /></td>
                        </tr>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Hike/Trail Name: </b></p></td>
                            <td><input class="form-control mr-sm-2 form-input" id="hike-name" name="trails" type="text" placeholder="Trailhead or hike..." /></td>
                        </tr>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Description </b>(optional): </p></td>
                            <td><input class="form-control mr-sm-2 form-input" id="hike-name" name="description" type="text" placeholder="Steep hike with rocky terrain..." /></td>
                        </tr>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Distance </b>(in miles): </p></td>
                            <td><input class="form-control mr-sm-2 form-input" id="distance" type="text" placeholder="ex: 7.2" /></td>
                        </tr>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Latitude: </b></p></td>
                            <td><input class="form-control mr-sm-2 form-input" id="latitude" type="text" placeholder="ex: 72.313" /></td>
                        </tr>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Longitude: </b></p></td>
                            <td><input class="form-control mr-sm-2 form-input" id="longitude" type="text" placeholder="ex: -30.354" /></td>
                        </tr>
                        <tr class="submit-row">
                            <td class="left-td"><p class="form-element"><b>Image</b> (optional): </p></td>
                            <td>                   
                                <ImageUploader
                                    withIcon={false}
                                    buttonText='Upload image'
                                    onChange={props.handleImageUpload.bind(this)}
                                    imgExtension={['.jpg', '.png']}
                                    maxFileSize={5242880}
                                    singleImage={true}
                                />
                            </td>
                        </tr>
                    </tbody>
              </table>
              <button id="submit-btn" class="btn btn-secondary my-2 my-sm-0" type="Submit">Submit</button>
            </form>
          </section>
        </div>
      );
}