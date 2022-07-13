import React from "react";
import jwt_decode from "jwt-decode";  

import NewApplicationButton from "../../Components/Buttons/NewApplication/NewApplicationButton";
import NavigationBar from "../../Components/NavigationBar/NavigationBar";
import NewApplicationPopup from "../../Components/Popup/NewApplicationPopup/NewApplicationPopup";

class Portal extends React.PureComponent {
    constructor(props){
        super(props); 
        this.token = sessionStorage.getItem('authorization');
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
            
        }
    }

    onClickNewApplication = () => {
        window.location = '/NewApplication';
    }

    render() {
        const renderNewApplicationButton = (
            <div className="new-application-button-wrapper">
                
                <NewApplicationPopup/>
            </div>
        );


        return(
            <div className ="portal-wrapper">
                <div className="portal-navbar-wrapper">
                    <NavigationBar/>
                </div>
               {renderNewApplicationButton}
            </div>
        );
    }
}

export default Portal;