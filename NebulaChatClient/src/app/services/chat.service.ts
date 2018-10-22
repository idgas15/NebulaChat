import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Message, TopTen } from '../models';

@Injectable()
export class ChatService {
    constructor(private http: HttpClient) { }

    getMessages(){
        return this.http.get<Message[]>(`${environment.apiUrl}/api/chat`);
    }
    sendMessage(message){
        return this.http.post(`${environment.apiUrl}/api/chat`, message);
    }
    getTopTenChatters(){
        return this.http.get<TopTen[]>(`${environment.apiUrl}/api/dashboard/topten`);
    }
}