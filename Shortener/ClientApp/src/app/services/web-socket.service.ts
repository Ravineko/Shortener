import { Injectable } from '@angular/core';
import { webSocket } from 'rxjs/webSocket';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  private socket$ = webSocket('wss://your-server-url');

  getSocket() {
    return this.socket$;
  }

  sendMessage(message: any): void {
    this.socket$.next(message);
  }
}
