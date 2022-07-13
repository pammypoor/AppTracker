import React from "react";
import Popup from "reactjs-popup";

import NewApplicationButton from "../../Buttons/NewApplication/NewApplicationButton";
import NewApplicationForm from "../../Forms/NewApplicationForm/NewApplicationForm";

import "./NewApplicationPopup.css";

class NewApplicationPopup extends React.Component {
    render() {
        return (
            <div className="new-application-popup-container">
                <Popup trigger={<NewApplicationButton type="button" name="New Application"/>} modal>
                    {close => (
                        <div className="new-application-popup-modal">
                            <button className="new-application-popup-close" onClick={close}>
                                &times;
                            </button>
                            <div className="new-application-popup-header">
                                New Application
                            </div>
                            <div className="new-application-popup-content">
                                    <NewApplicationForm/>
                            </div>
                        </div>
                    )}
                </Popup>
            </div>
        )
    }
}

export default(NewApplicationPopup);