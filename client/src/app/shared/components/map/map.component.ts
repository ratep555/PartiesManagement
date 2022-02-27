import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { icon, latLng, LeafletMouseEvent, marker, Marker, tileLayer } from 'leaflet';
import { CoordinatesMap, CoordinatesMapWithMessage } from '../../models/coordinate';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  @Output() selectedLocation = new EventEmitter<CoordinatesMap>();
  @Input()  initialCoordinates: CoordinatesMapWithMessage[] = [];
  @Input() editMode: boolean = true;
  model: CoordinatesMap = {latitude: 45.823191360974505,
                           longitude: 16.00924925704021 };

layers: Marker<any>[] = [];
  options = {
    layers: [
      tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: 'Office Location' })
    ],
    zoom: 7,
    center: latLng(44.815183840035594, 15.99454538427381)
  };

  constructor() { }

  ngOnInit(): void {
    this.layers = this.initialCoordinates.map((value) => {
      const m = marker([value.latitude, value.longitude]);
      if (value.message){
        m.bindPopup(value.message, {autoClose: false, autoPan: false});
      }
      return m;
    });
  }

  handleMapClick(event: LeafletMouseEvent) {
    if (this.editMode) {
      const latitude = event.latlng.lat;
      const longitude = event.latlng.lng;
      console.log({ latitude, longitude });
      this.layers = [];
      this.layers.push(marker([latitude, longitude]));
      this.selectedLocation.emit({ latitude, longitude });
    }

  }



}
