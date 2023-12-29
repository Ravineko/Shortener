import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface LinkModel {
  id: number;
  originalLink: string;
  shortenedLink: string;
  info: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  links: LinkModel[] = [];

  showForm = false;
  newUrl: string = '';

  constructor(private http: HttpClient) { }

  showAddUrlForm(): void {
    this.showForm = true;
  }
  addUrl(): void {
    this.http.post('https://localhost:44367/links/add', { originalLink: this.newUrl })
      .subscribe(() => {
        this.fetchLinks(); // Оновити список посилань після успішного додавання
        this.showForm = false; // Сховати форму після успішного додавання
        this.newUrl = ''; // Очистити поле вводу
      });
  }
  ngOnInit(): void {
    this.http.get<LinkModel[]>('https://localhost:44367/links')
      .subscribe(data => this.links = data);
  }
  fetchLinks(): void {
    this.http.get<LinkModel[]>('https://localhost:44367/links')
      .subscribe(data => this.links = data);
  }
}
