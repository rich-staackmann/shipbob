import Vue from 'vue';
import axios from 'axios';
import store from '../../store';
import { Component } from 'vue-property-decorator';

interface User {
userId: number;
firstName: string;
lastName: string;
}

@Component
  export default class UserListComponent extends Vue {
  users: User[] = [];
  firstName: string = "";
  lastName: string = "";
  errors: string[] = [];
    
  selectUser(userId: number): void {
      store.setMessageAction(userId);     
  }
  mounted() {
    fetch('api/users')
        .then(response => response.json() as Promise<User[]>)
        .then(data => {
            this.users = data;
        });
    }
}