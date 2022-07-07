import React from "react";
import { FaPlusCircle} from 'react-icons/fa';
import "./NewApplicationButton.css";

const NewApplicationButton = ({ name = "New Application", disabled = false, onClick}) => {
    const onButtonClick = () => {
        if(onClick) {
            onClick();
        }
    }
    
    return (
        <div className="new-application-button-container">
            <button className= "new-application-button" onClick={onButtonClick}><span className="new-application-button-text">&#43;&emsp;{name}</span><span><FaPlusCircle/></span></button>
        </div>
    )    
}

export default NewApplicationButton;