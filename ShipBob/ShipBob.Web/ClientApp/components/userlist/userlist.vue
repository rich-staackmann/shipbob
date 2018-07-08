<template>
    <div>
        <h3>List of Users</h3>
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>First</th>
                    <th>Last</th>
                    <th>Select User</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in users">
                    <td>{{ item.userId }}</td>
                    <td>{{ item.firstName }}</td>
                    <td>{{ item.lastName }}</td>
                    <td><button v-on:click="selectUser(item.userId)">Select</button></td>
                </tr>
            </tbody>
        </table>       
    </div>
</template>

<script lang=""ts"">
  import Vue from 'vue';
  import axios from 'axios';
  import store from '../../store'
  import { Component } from 'vue-property-decorator';

  type User {
  userId: number;
  firstName: string;
  lastName: string;
  }

  @Component
  export default class UserList extends Vue {
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
</script>