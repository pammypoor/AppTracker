import React from "react";
import axios from "axios";
import "./NewApplicationForm.css";

class NewApplicationForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            token: sessionStorage.getItem('authorization'),
            errorMessage: '',
            company: '', 
            position: ''

        }
    }

    inputCompanyHandler = (e) => {
        this.setState({company: e.target.value});
    }

    inputPositionHandler = (e) => {
        this.setState({position: e.target.value});
    }

    onSubmitHandler = (e) => {
        e.preventDefault();
        if(this.validateInput()){
            this.setState({errorMessage:''});
            axios.post()
            .then(response => {
                
            })
            .catch(err => {
            })
        }
    }

    render() {
        return (
            <div className="new-application-form-container">
                <form className="new-application-form" onSubmit = {this.onSubmitHandler}>
                    <div className="new-application-company-field">
                        <input type="text" value = {this.state.company} required placeholder="Company" onChange = {this.inputCompanyHandler}/>
                    </div>
                    <div className="new-application-position-field">
                        <input type="text" value={this.state.position} required placeholder="Position" onChange={this.inputPositionHandler}/>
                    </div>
                    <div className="new-application-button-field">
                        <button className="new-application-button">Create</button>
                    </div>
                    {this.state.errorMessage && <div className="error-signin">{this.state.errorMessage}</div>}
                </form>
            </div>
        )
    }
}

export default NewApplicationForm;