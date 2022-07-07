import React from "react";
import jwt_decode from "jwt-decode";  

import NewApplicationForm from "../../Components/Forms/NewApplicationForm/NewApplicationForm";

import "./NewApplication.css";

class NewApplication extends React.PureComponent {
    constructor(props){
        super(props); 
    }

    componentDidMount() {
        this.checkToken();
    }

    checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token) {
            const decoded = jwt_decode(token);
            const tokenExpiration = decoded.tokenExpiration;
            const now = new Date();

            if(now.getTime() > decoded.exp * 1000){
                localStorage.removeItem('authorization');
                window.location = '/';
            }
        } else {
            // Token doesn't exist or not valid
            localStorage.removeItem('authorization');
            window.location = '/';
        }
    }

    
    render() {
        const renderApplicationForm = (
            <div className="new-application-form-wrapper">
                <NewApplicationForm/>
            </div>

        );
        return(
            <div className ="new-application-wrapper">
                {renderApplicationForm}
            </div>
        );
    }
}

export default NewApplication;