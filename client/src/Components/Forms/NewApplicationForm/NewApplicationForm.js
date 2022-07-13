import React from "react";
import axios from "axios";
import "./NewApplicationForm.css";

import PositionInfoForm from "./PositionInfoForm";

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
        const renderProgressBar = (
            <div className="new-application-form-progress">
                
            </div>
        )        

       

        


        return (
            <div className="new-application-form-container">
                {renderProgressBar}
            </div>

        )
    }
}

export default NewApplicationForm;