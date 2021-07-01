import { IInputs, IOutputs } from "./generated/ManifestTypes";
import * as React from "react";
import * as ReactDOM from "react-dom";
import SurveyContainer, { ISurveyContainerProps } from "./components/survey-container";
import {Environment} from "./utils/constants"

export class SurveyManagementControl
  implements ComponentFramework.StandardControl<IInputs, IOutputs>
{
  private _notifyOutputChanged: () => void;
  private _container: HTMLDivElement;
  
  private _props : ISurveyContainerProps = {
    surveyRequestId: ""
  };

  /**
   * Empty constructor.
   */
  constructor() {}

  /**
   * Used to initialize the control instance. Controls can kick off remote server calls and other initialization actions here.
   * Data-set values are not initialized here, use updateView.
   * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to property names defined in the manifest, as well as utility functions.
   * @param notifyOutputChanged A callback method to alert the framework that the control has new outputs ready to be retrieved asynchronously.
   * @param state A piece of data that persists in one session for a single user. Can be set at any point in a controls life cycle by calling 'setControlState' in the Mode interface.
   * @param container If a control is marked control-type='standard', it will receive an empty div element within which it can render its content.
   */
  public init(
    context: ComponentFramework.Context<IInputs>,
    notifyOutputChanged: () => void,
    state: ComponentFramework.Dictionary,
    container: HTMLDivElement
  ): void {
    // Add control initialization code

    if(Environment.IsDev)
      console.log("Develoment mode is active.");

    this._notifyOutputChanged = notifyOutputChanged;
    this._container = document.createElement("div");
    this._container.classList.add("smcc");
    container.appendChild(this._container);
  }

  /**
   * Called when any value in the property bag has changed. This includes field values, data-sets, global values such as container height and width, offline status, control metadata values such as label, visible, etc.
   * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to names defined in the manifest, as well as utility functions
   */
  public updateView(context: ComponentFramework.Context<IInputs>): void {
    // Add code to update control view
debugger
    if(context.parameters.surveyRequestProperty.raw?.length > 0 &&
      context.parameters.surveyRequestProperty.raw[0].id !== this._props.surveyRequestId){
        this._props.surveyRequestId = context.parameters.surveyRequestProperty.raw[0].id;

        //...... todo
      }

      if(Environment.IsDev){
        this._props.surveyRequestId = "4f057b0-bbc2-eb11-bacc-0022489c58e9";
      }


      ReactDOM.render(React.createElement(SurveyContainer, this._props), this._container);
  }

  /**
   * It is called by the framework prior to a control receiving new data.
   * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as “bound” or “output”
   */
  public getOutputs(): IOutputs {
    debugger
    return {};
  }

  /**
   * Called when the control is to be removed from the DOM tree. Controls should use this call for cleanup.
   * i.e. cancelling any pending remote calls, removing listeners, etc.
   */
  public destroy(): void {
    // Add code to cleanup control if necessary
    ReactDOM.unmountComponentAtNode(this._container);
  }
}
