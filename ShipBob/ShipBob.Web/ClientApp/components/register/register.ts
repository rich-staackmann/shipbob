﻿import Vue from 'vue';
import axios from 'axios';
import store from '../../store'
import { Component, Prop } from 'vue-property-decorator';

interface User {
    userId: number;
    firstName: string;
    lastName: string;
}

@Component
export default class RegisterUserComponent extends Vue {
    @Prop({ default: 0})
    currentUserId: number = 0;
    users: User[] = [];
    firstName: string = "";
    lastName: string = "";
    errors: string[] = [];
    registerUser(): void {
        var user: User =
        {
            userId: 0,
            firstName: this.firstName,
            lastName: this.lastName
            };
        //https://www.linkedin.com/pulse/post-data-from-vuejs-aspnet-core-using-axios-adeyinka-oluwaseun/
        axios({
            method: 'post',
            url: 'http://localhost:51743/api/users',
            data: {
                "firstName": this.firstName,
                "lastName": this.lastName
            }
        })
        .then((response) => {
            this.users.push(response.data);
            store.setMessageAction(response.data.userId);
            console.log(response);
            console.log(this.currentUserId);
        })
        .catch(function (error) {
            console.log(error);
        });       
    }
    mounted() {
        fetch('api/users')
            .then(response => response.json() as Promise<User[]>)
            .then(data => {
                this.users = data;
            });
    }
}