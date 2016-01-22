/**
 * Created by BuldyginRA on 15.01.2016.
 */

import nanomsg.reqrep.RepSocket;

import java.io.UnsupportedEncodingException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.atomic.AtomicBoolean;

public class NanoMsgServer {
    private RepSocket _repSocket;
    private ExecutorService executor;

    public NanoMsgServer() {
        executor = Executors.newSingleThreadExecutor();
        _repSocket = new RepSocket();
        _repSocket.setRecvTimeout(-1);
        System.out.println("Сервер создан");
    }

    public void start()
    {
        executor.submit( () -> {
            try{
                _repSocket.bind("tcp://*:6789");

                System.out.println("Сервер начинает прослушивание");
                while (!Thread.interrupted()) {
                    byte[] receivedData = _repSocket.recvBytes();
                    System.out.println(new String(receivedData,"UTF-8"));
                    _repSocket.send(receivedData);
                }
            } catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            } finally {
                _repSocket.close();
            }
        });
    }

    public void stop() {
        executor.shutdownNow();
        try {
            executor.awaitTermination(1, TimeUnit.SECONDS);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
}
