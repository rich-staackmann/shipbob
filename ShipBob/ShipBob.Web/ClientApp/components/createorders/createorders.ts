import Vue from 'vue';
import axios from 'axios';
import { Component } from 'vue-property-decorator';
import store from '../../store'


@Component
export default class CreateOrderComponent extends Vue {
    trackingId: string = "";
    name: string = "";
    street: string = "";
    city: string = "";
    state: string = "";
    zipCode: string = "";
    errors: string[] = [];

    createOrder(): void {
        //https://www.linkedin.com/pulse/post-data-from-vuejs-aspnet-core-using-axios-adeyinka-oluwaseun/
        axios({
            method: 'post',
            url: 'http://localhost:51743/api/orders',
            data: {
                "userId": store.state.id,
                "trackingId": this.trackingId,
                "name": this.name,
                "street": this.street,
                "city": this.city,
                "state": this.state,
                "zipCode": this.zipCode
            }
        })
            .then((response) => {
                console.log(response);
            })
            .catch(function (error) {
                console.log(error);
            });
    }
}