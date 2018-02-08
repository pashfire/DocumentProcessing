"use strict";
var resolutionPageVueInstance;


document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    resolutionPageVueInstance = new Vue({
        el: '#taskPage',
        data: () => ({
            
            form: {
                Id: null,
                Name: null,
                Path: null,
                CreatorId: null,
                RegistrationDate: null,
                Created: null,
                Creator: null,
                Type: null,
                AllTypes: null,
                ExecutionPeriod: null,
                NomenclatureId: null,
                DocIndex: null,
                DocHeader: null,
                ManagerId: null,
                Resolution: null,
                ExecutorId: null,
                ExecutorNote: null,
                ControllerId: null,
                ControllerNote: null,
                DocumentFile: null,
                Managers: null,
                AllNomenclature: null,
                
                Executors: null,
                Inspectors: null
                
            },
            editing: true,
            sending: false,
            lastUser: null
        }),
       
        //computed: {
        //    orderedTypes: function () {
        //        return this.AllTypes
        //    }
        //    //creationDate: function () {
        //    //    return new Date()
        //    //},
        //    //regDate: function () {
        //    //    return new Date()
        //    //},
        //    //expirationDate: function () {
        //    //    return new Date()
        //    //},
        //},

        mounted: function () {
            this.editing = !window.model.IsDocumentExists;
            this.form = window.model.DocumentModel;
        },

        validations: {
            form: {
                Resolution: {
                    required: validators.required,
                },
                ExecutorId: {
                    required: validators.required
                },
                ControllerId: {
                    required: validators.required
                }
            }
        },
        //computed: {
        //    orderedTypes: function () {
        //        return _.orderBy(this.AllTypes, 'Name')
        //    },

        methods: {
            getValidationClass(fieldName) {
                const field = this.$v.form[fieldName]

                if (field) {
                    return {
                        'md-invalid': field.$invalid && field.$dirty
                    }
                }
            },
            orderedTypes: function () {
                return _.orderBy(this.AllTypes, 'Name')
            },

            

            saveDocument() {
                this.sending = true;
                var _this = this;
                var regDate = new Date();
                var formData = new FormData(document.getElementById('documentForm'));
                formData.append("Id", this.form.Id);
                formData.append("Name", this.form.Name);
                formData.append("DocIndex", this.form.DocIndex);
                formData.append("Path", this.form.Path);
                formData.append("CreatorId", this.form.CreatorId);
                formData.append("Created", this.form.Created);
                formData.append("Type", this.form.Type);
                formData.append("ExecutionPeriod", this.form.ExecutionPeriod);
                formData.append("RegistrationDate", this.form.RegistrationDate);
                formData.append("ManagerId", this.form.ManagerId);
                formData.append("NomenclatureId", this.form.NomenclatureId);
                formData.append("DocHeader", this.form.DocHeader);

                formData.append("Resolution", this.form.Resolution);

                formData.append("ExecutorId", this.form.ExecutorId);
                formData.append("ExecutorNote", this.form.ExecutorNote);
                formData.append("ControllerId", this.form.ControllerId);

                Vue.http.post('/Document/Task/', formData)
                    .then(response => {
                        //success callback
                        _this.documentCreated = true;
                        this.lastUser = `${this.form.firstName} ${this.form.lastName}`;
                        this.sending = false;
                        if (this.userSaved) {
                            document.location.href = "/";
                        }
                    }, response => {
                        // error callback
                        _this.documentCreated = false;
                        this.sending = false;
                    });
                Vue.http.get('/Document/Tasks/');
            },
            validateDocument() {
                this.$v.$touch();

                if (!this.$v.$invalid) {
                    this.saveDocument();
                }
            }
        }

    });
});