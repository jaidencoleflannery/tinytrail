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
  public total: number | null = null;

  public links: any[] | null = null;
  public linksSubscription!: Subscription;
  private linksSubject = new BehaviorSubject<string[]>([""]);
  public links$: Observable<string[]> = this.linksSubject.asObservable().pipe(filter((val): val is string[] => val !== null));

  public username: string | null = null;
  public usernameSubscription!: Subscription;
  private currentUsernameSubject = new BehaviorSubject<string>("");
  public currentUsername$: Observable<string> = this.currentUsernameSubject.asObservable().pipe(filter((val): val is string => val !== null));

  constructor() {}

  async ngOnInit(){
    this.links = await this.GetLinks();
    this.username = await this.GetUsername();

    this.usernameSubscription = this.currentUsername$.subscribe({
      next: (newUsername: string) => {
        this.username = newUsername;
      },
      error: (err: any) => {
        console.error('Username observable error:', err);
      }
    });

    this.linksSubscription = this.links$.subscribe({
      next: (newLinks: string[]) => {
        this.links = newLinks;
      },
      error: (err: any) => {
        console.error('Links observable error:', err);
      }
    });
  }

   async GetUsername(): Promise<string>{
    let url: string = "http://localhost:5137/api/auth/status";
    try{
      const response = await fetch(url, {
          method: "GET",
          credentials: 'include',
          headers: { 
            "content-type": "application/json" 
          },
        });
        const data = await response.json();
        this.currentUsernameSubject.next(data);
        return data.user;
    } catch (err) {
      console.log(err);
      throw Error("(Profile) Could not get username.");
    }
  }

  async GetLinks(): Promise<string[]>{
    let url: string = "http://localhost:5137/api/Link/GetUsersLinks";
    try{
      const response = await fetch(url, {
          method: "GET",
          credentials: 'include',
          headers: { 
            "content-type": "application/json" 
          },
        });
        const data = await response.json();
        this.total = data.length;
        this.linksSubject.next(data);
        return data;
    } catch (err) {
      throw Error("(Profile) Could not get users links.");
    }
  }

  async DeleteLink(link: string) {
    let url: string = "http://localhost:5137/api/Link/DeleteLink?url=" + link;
    try{
      const response = await fetch(url, {
          method: "POST",
          credentials: 'include',
          headers: { 
            "content-type": "application/json" 
          },
        });
        const data = await response.json();
        this.total = this.total! - 1;
        console.log(data);
        return data;
    } catch (err) {
      throw Error("(Profile) Could not delete link.");
    }
  }
}
