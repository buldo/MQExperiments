/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.buldygin.javaserver;
import org.zeromq.ZMQ;
import org.zeromq.ZContext;
import org.zeromq.ZMQ.Context;
import org.zeromq.ZMQ.Socket;

/**
 *
 * @author buldo
 */
public class DomainModel {
    private final ZContext _context;
    private final Socket _socket;
    
    public DomainModel() {
        _context = new ZContext();
        _socket = _context.createSocket(ZMQ.PUB);
        _socket.bind("tcp://*:1713");
    }
    
    public void sendText(String toSend)
    {
        _socket.send(toSend);
    }
}
