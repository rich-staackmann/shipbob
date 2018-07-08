import Vue from 'vue';
import axios from 'axios';
import { Component } from 'vue-property-decorator';
import store from '../../store';

interface Order {
    orderId: number;
    userId: number;
    trackingId: string;
    name: string;
    street: string;
    city: string;
    state: string;
    zipCode: string;
}

@Component({
    components: {
        UserListComponent: require('../userlist/userlist.vue.html')
    }
})
export default class OrderComponent extends Vue {
    orders: Order[] = [];
    userId: number = store.state.id;
    trackingId: string = "";
    name: string = "";
    street: string = "";
    city: string = "";
    state: string = "";
    zipCode: string = "";
    errors: string[] = [];

    updateOrder(order: Order): void {
        //https://www.linkedin.com/pulse/post-data-from-vuejs-aspnet-core-using-axios-adeyinka-oluwaseun/
        axios({
            method: 'put',
            url: 'http://localhost:51743/api/orders/' + order.orderId,
            data: {
                "orderId": order.orderId,
                "userId": store.state.id,
                "trackingId": order.trackingId,
                "name": order.name,
                "street": order.street,
                "city": order.city,
                "state": order.state,
                "zipCode": order.zipCode
            }
        })
            .then((response) => {
                console.log(response);
            })
            .catch(function (error) {
                console.log(error);
            });
    }
    updateUser(userId: number): void {
        store.setMessageAction(userId);
        axios({
            method: 'get',
            url: 'http://localhost:51743/api/orders/user/' + userId
        })
            .then((response) => {
                this.orders = response.data;
                console.log(response);
            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
    }
    mounted() {
        //fetch('http://localhost:51743/api/orders/user/' + store.state.id)
        //    .then(response => response.json() as Promise<Order[]>)
        //    .then(data => {
        //        this.orders = data;
        //    });
    }
}
