import { Component, OnInit } from '@angular/core';
import {FormControl, ReactiveFormsModule} from '@angular/forms';
import { NavigationComponent } from "./navigation/navigation.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [ReactiveFormsModule, NavigationComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent{
  link = new FormControl();
}
