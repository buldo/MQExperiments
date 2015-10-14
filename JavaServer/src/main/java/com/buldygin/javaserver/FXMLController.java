package com.buldygin.javaserver;

import java.net.URL;
import java.util.ResourceBundle;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;

import javafx.scene.control.TextArea;

public class FXMLController implements Initializable {
    private DomainModel _domain;
    
    @FXML
    private TextArea text;
    
    public FXMLController() {
    }            
    
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        // TODO
    }
    
    @FXML
    private void handleButtonAction(ActionEvent event) {
        //System.out.println("You clicked me!");
        //label.setText("Hello World!");
        String txt =  text.getText();
        text.clear();
        _domain.sendText(txt);
    }     
    
    public void setController(DomainModel model) {
        _domain = model;
    }
}
