import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {}

  loginForm!: FormGroup;

  ngOnInit() {
    this.loginForm = this.fb.nonNullable.group({
      username: ['', [Validators.required, Validators.minLength(1)]],
      password: ['', [Validators.required, Validators.minLength(1)]],
    });
  }

  async submit() {
    console.log(this.loginForm.value);
    console.log("SUBMITTED");
    if(this.loginForm.invalid) {
      console.log("invalid");
      this.loginForm.markAllAsTouched();
      return;     
    }
    console.log("grabbing creds");
    const username = this.loginForm.value.username; 
    const password = this.loginForm.value.password; 
    const res = await this.authService.login(username, password);
    if(res == true) {
      console.log("rerouting");
      this.router.navigate(['/main']).catch(err => console.error('UNABLE TO REROUTE:', err));
    }
    this.loginForm.reset({ username: '', password: '' });
  }
}