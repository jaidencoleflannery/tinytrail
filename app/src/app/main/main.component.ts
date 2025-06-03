import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormControl } from '@angular/forms';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent{
  value: any;
  form: any;

  link = new FormControl();

  constructor (private fb: FormBuilder) {
    this.form = this.fb.group({
      value: ''
    })
  }

    async submit () {
    if (this.form.get('value').value == null || this.form.get('value').value == '' || this.form.get('value').value == ' ') {
      return 0;
    } else {
      const url = `http://localhost:5137/api/Link/TransformLink?url=` + this.form.get('value').value;
      try {
        const response = await fetch(url, {
          method: "POST",
          credentials: 'include',
          headers: { 
            "content-type": "application/json" 
          },
        });
        if (!response.ok) {
          throw new Error(response.statusText);
        }
        const data = await response.json();
        console.log("DATA:", data);
      } catch (error) {
        console.log(error);
      }
      this.form.reset(
        this.value = ''
      );
      return null;
    }
  }

  dynamicInput(textarea: any): void {
      textarea.style.height = "auto";
      textarea.style.height = textarea.scrollHeight + "px";
  }

  handleEnter(event: KeyboardEvent, textarea: any): void {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.submit();
      this.dynamicInput(textarea);
    }
  }
}
