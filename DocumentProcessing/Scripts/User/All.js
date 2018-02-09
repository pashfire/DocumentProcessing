"use strict";
var userPageVueInstance;


document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    userPageVueInstance = new Vue({
        el: '#usersPage',
        data: () => ({
            users: {
                AllUsers: null,
                Id: null,
                User: null,
            },
            editing: true,
            sending: false,
            lastUser: null
        }),

        mounted: function () {
            this.editing = !window.model.IsUserExists;
            this.users = window.model.AllUsersModel;
        },

        methods: {
            
            orderedTypes: function () {
                return _.orderBy(this.AllTypes, 'Name')
            },
            toLower(text) {
                return text.toString().toLowerCase()
            },

            searchByName(items, term) {
                if (term) {
                    return items.filter(function (item) {
                        return toLower(item.name).includes(toLower(term))
                    })
                }

                return items
            },

            searchOnTable: function searchOnTable() {
                this.searched = searchByName(this.users, this.search)
            },
        }

    });
});