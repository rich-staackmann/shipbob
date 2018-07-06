import Vue from 'vue';
import axios from 'axios';
import { Component } from 'vue-property-decorator';

interface User {
    userId: number;
    firstName: string;
    lastName: string;
}

@Component
export default class RegisterUserComponent extends Vue {
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
            console.log(response);
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