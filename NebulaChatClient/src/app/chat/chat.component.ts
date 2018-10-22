import { Component, OnInit, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User, Message, TopTen } from '../models';
import { AlertService, ChatService, UserService } from '../services';
import { first } from 'rxjs/operators';
import * as signalR from "@aspnet/signalr";
import { environment } from 'src/environments/environment';

@Component({
    selector: 'chat',
    templateUrl: 'chat.component.html'
})

export class ChatComponent implements OnInit {
    chatForm: FormGroup;
    loading = false;
    submitted = false;
    currentUser = JSON.parse(localStorage.getItem('currentUser'));
    messages : Message[]= [];
    allUsers: User[] = [];
    users :User[] = [];
    topTen = [];

    constructor(
        private formBuilder: FormBuilder,
        private chatService: ChatService,
        private userService: UserService,
        private alertService: AlertService,
        private myElement: ElementRef
    ) {
        this.userService.getAll().subscribe(_users=> {
            this.setUsers(_users)
        });
        this.chatService.getMessages().subscribe(data=> {
            this.messages = data;
        });
        this.chatService.getTopTenChatters().subscribe(data => {
            this.topTen = data;
        });
        this.chatForm = this.formBuilder.group({
            message: ['', Validators.required],
            recipientId: [null, Validators.required]
        });
    }
    ngOnInit() { 
        const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${environment.apiUrl}/chatHub`)
        .build();
        connection.start().catch(err => document.write(err));
        connection.on("broadcastMessage", (messageNotification) => {
            if(messageNotification){
                this.messages.push(messageNotification);
            }
        });
        connection.on("PostedMessagesRefresh", (messageNotifications) => {
            if(messageNotifications){
                this.messages = [];
                this.messages = messageNotifications;
            }
        });
        connection.on("UsersRefresh", (refreshedUsers) => {
            if(refreshedUsers){
                this.setUsers(refreshedUsers)
            }
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.chatForm.controls; }

    onSubmit() {
        debugger;
        this.submitted = true;

        // stop here if form is invalid
        if (this.chatForm.invalid) {
            return;
        }

        this.loading = true;
        const formValues = (this.chatForm.value);
        const payload = {
            authorId: this.currentUser.id,
            recipientId: formValues.recipientId,
            content: formValues.message,
            createdDate: null
        }
        this.chatService.sendMessage(payload)
            .pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    this.chatForm.reset();
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
    getUser(value: number){
        debugger;
    }

    getPopoverTitle(user) {
        if(user){
            return user.firstName + ' ' + user.lastName;
        }
        
    }
    getPopOverBody(user) {
        if(user){
            let popoverBody: string;
            popoverBody = `Email: ${user.username}`
            return popoverBody;
        }
    }

    private setUsers(users){
        if(users){
            this.allUsers = [];
            this.allUsers = users;
            this.users = [];
            this.users = users.filter((value, index, array)=> {
                return value.id !== this.currentUser.id;
            })
        }
    }
}