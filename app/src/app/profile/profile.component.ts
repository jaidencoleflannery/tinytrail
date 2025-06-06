import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { BehaviorSubject, Observable, filter } from 'rxjs';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NavigationComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  public links: any[] = [];
  public total: number | null = null;
  public username: string | null = null;
  public usernameSubscription!: Subscription;
  private readonly url: string = "http://localhost:5137/api/Link/GetUsersLinks";

  private currentUsernameSubject = new BehaviorSubject<string>("");
  public currentUsername$: Observable<string> = this.currentUsernameSubject.asObservable().pipe(filter((val): val is string => val !== null));

  constructor() {}

  async ngOnInit(){
    this.usernameSubscription = this.currentUsername$.subscribe({
      next: (newUsername: string) => {
        this.username = newUsername;
      },
      error: (err: any) => {
        console.error('Username observable error:', err);
      }
    });

    this.links = await this.GetLinks();
    this.username = await this.GetUsername();
  }

   async GetUsername(): Promise<string>{
    let url = "http://localhost:5137/api/auth/status";
    try{
      const response = await fetch(url, {
          method: "GET",
          credentials: 'include',
          headers: { 
            "content-type": "application/json" 
          },
        });
        const data = await response.json();
        return data.user;
    } catch (err) {
      console.log(err);
      throw Error("(Profile) Could not get username.");
    }
  }

  async GetLinks(): Promise<string[]>{
    try{
      const response = await fetch(this.url, {
          method: "GET",
          credentials: 'include',
          headers: { 
            "content-type": "application/json" 
          },
        });
        const data = await response.json();
        this.total = data.length;
        console.log(this.total);
        console.log(data);
        return data;
    } catch (err) {
      throw Error("(Profile) Could not get users links.");
    }
  }

  async CopyLink() {
    
  }
}
