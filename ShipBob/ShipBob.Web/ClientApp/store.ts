const store = {
    debug: true,
    state: {
        id: 0
    },
    setMessageAction(newValue: number) {
        this.debug && console.log('setMessageAction triggered with', newValue)
        this.state.id = newValue
    },
    clearMessageAction() {
        this.debug && console.log('clearMessageAction triggered')
        this.state.id = 0
    }
};

export default store;