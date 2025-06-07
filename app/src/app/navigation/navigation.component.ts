import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BehaviorSubject, Observable, filter, Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [ RouterModule ],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements OnInit {
  public loggedIn: boolean = false;
  public loggedInSub!: Subscription;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.loggedInSub = this.authService.isAuthenticated$.subscribe({
      next: (status: boolean) => {
        this.loggedIn = status;
      },
      error: (err: any) => {
        console.error('(nav) User status observable error:', err);
      }
    });
  }

  async logout() {
    const res = await this.authService.logout();
  }

}
