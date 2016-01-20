import nanomsg.reqrep.ReqSocket;

import java.io.Console;
import java.util.Objects;
import java.util.Scanner;

/**
 * Created by BuldyginRA on 18.01.2016.
 */
public class ClientRunner {
    public static void main(String[] args) {
        ReqSocket sock = new ReqSocket();
        sock.setSendTimeout(-1);
        sock.setRecvTimeout(-1);
        sock.connect("tcp://localhost:6789");

        Scanner scanner = new Scanner(System.in);

        System.out.println("Введите \"exit\" для выхода ");
        while(true)
        {
            String line = scanner.nextLine();
            if(Objects.equals(line, "exit"))
            {
                break;
            }

            sock.send(line);
            System.out.println("Received: " + sock.recvString());
        }

        System.out.println("Завершение работы");
        sock.close();
    }
}
